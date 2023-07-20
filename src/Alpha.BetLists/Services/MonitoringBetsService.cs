namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Helpers;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Alpha.PartialUM.Services;
    using Alpha.Web.Models;
    using Fanex.Data.Repository;

    public interface MonitoringBetsService
    {
        SavedFilter BuildSavedFilter(AddFilterParam param);

        Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId);

        Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId);

        Task<Result> AddFilter(AddFilterParam param);

        Task<Result> UpdateFilter(UpdateFilterParam param);

        Task<IEnumerable<Result>> DeleteFilters(DeleteFiltersParam param);

        Task<IEnumerable<Sport>> GetSports(byte sportGroup);

        Task<IEnumerable<MemberErrCode>> CheckUsernames(int agentId, string usernames);

        Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId);
        Task<IEnumerable<TicketDto>> GetLastFilteringBets(GetLastFilteringBetsParams param);

    }

    public class MonitoringBetsServiceImpl : MonitoringBetsService
    {
        private const int InvalidUsername = 100;
        private const int NotAMember = 101;
        private const string IgnoreSportTypeName = "upcoming";
        private static readonly IDictionary<int, bool> MajorSports = new Dictionary<int, bool>
        {
            [SportTypeIdConstants.Soccer] = true,
            [SportTypeIdConstants.Basketball] = true,
            [SportTypeIdConstants.Badminton] = true,
            [SportTypeIdConstants.ESports] = true,
            [SportTypeIdConstants.Tennis] = true
        };
        private readonly IPartialUMService partialUMService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly LastBetsService lastBetsService;

        public MonitoringBetsServiceImpl(
            IPartialUMService partialUMService,
            LastBetsService lastBetsService,
            IDynamicRepository dynamicRepository)
        {
            this.partialUMService = partialUMService;
            this.lastBetsService = lastBetsService;
            this.dynamicRepository = dynamicRepository;
        }

        public SavedFilter BuildSavedFilter(AddFilterParam param)
        {
            var (sportTypes, leagues) = BuildSportTypesAndLeagues(param.Sports);

            return new SavedFilter(
                0,
                param.FilterName,
                false,
                param.BetType,
                param.Stake,
                param.LiveStatus,
                sportTypes,
                leagues,
                BuildMembers(param.FilterByMembers, param.InputList, param.MyWatchlist),
                param.FilterByMembers);
        }

        public async Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId)
        {
            var criteria = new GetFiltersCriteria(custId, subAccId);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var filterConfigurations = await dynamicRepository.FetchAsync<FilterConfiguration>(criteria);
            return filterConfigurations.OrderByDescending(f => f.FilterId).Select(f =>
            {
                var filterXml = f.FilterXml;
                var (sportTypes, leagues) = BuildSportTypesAndLeagues(filterXml.Sports);

                return new SavedFilter(
                    f.FilterId,
                    f.FilterName,
                    f.IsDefault,
                    filterXml.BetType,
                    filterXml.Stake,
                    filterXml.IsLive,
                    sportTypes,
                    leagues,
                    BuildMembers(filterXml.IsWatchlist, filterXml.Members, filterXml.MyWatchlistMembers),
                    filterXml.IsWatchlist);
            });
        }

        private static Tuple<string, string> BuildSportTypesAndLeagues(IEnumerable<SportXml> sportXmlList)
        {
            var sportTypes = new HashSet<string>();
            var leagues = new HashSet<string>();

            foreach (var item in sportXmlList)
            {
                if (!item.Sport.Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var sport in item.Sport.SplitByCommaWithNoneEmptyEntries())
                    {
                        sportTypes.Add(sport);
                        foreach (var league in item.League.SplitByCommaWithNoneEmptyEntries())
                        {
                            if (!league.Equals("0", StringComparison.OrdinalIgnoreCase))
                            {
                                leagues.Add(league);
                            }
                        }
                    }
                }
            }

            var sportTypeIds = sportTypes.Count == 0 ? "0" : string.Join(",", sportTypes);
            var leagueIds = leagues.Count == 0 ? "0" : string.Join(",", leagues);

            return Tuple.Create(sportTypeIds, leagueIds);
        }

        private static string BuildMembers(byte filterByMembers, string members, string myWatchlistMembers)
        {
            if (filterByMembers == FilterByMembersConstants.AllMembers)
            {
                return members;
            }

            if (filterByMembers == FilterByMembersConstants.AllWatchlistMembers)
            {
                return members;
            }

            return $"{members ?? string.Empty},{myWatchlistMembers ?? string.Empty}".TrimStart(',');
        }

        public async Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId)
        {
            var criteria = new GetFiltersCriteria(custId, subAccId);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var filterConfigurations = await dynamicRepository.FetchAsync<FilterConfiguration>(criteria);
            foreach (var item in filterConfigurations)
            {
                var filterXml = item.FilterXml;
                filterXml.BetTypeNames = BuildBetTypeNames(filterXml.BetType);
                filterXml.FlatternSports = BuildSports(FlattenSports(filterXml.Sports)).ToList();
            }

            return filterConfigurations;
        }

        private static IEnumerable<string> BuildBetTypeNames(string betTypes)
            => betTypes.Equals("0", StringComparison.OrdinalIgnoreCase)
            ? Enumerable.Empty<string>()
            : betTypes.ToListInt().Select(t => BetTypeHelper.GetBetTypeName(t)).ToList();

        private static IEnumerable<League> FlattenSports(IEnumerable<SportXml> sportXmlList)
        {
            foreach (var item in sportXmlList)
            {
                if (!item.Sport.Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var sport in item.Sport.SplitByCommaWithNoneEmptyEntries())
                    {
                        foreach (var league in item.League.SplitByCommaWithNoneEmptyEntries())
                        {
                            yield return new League(Convert.ToInt32(sport), Convert.ToInt32(league));
                        }
                    }
                }
            }
        }

        private static IEnumerable<Sport> BuildSports(IEnumerable<League> leagueList)
        {
            var groupBySportTypes = leagueList.GroupBy(l => l.SportType).OrderBy(l => l.Key);

            foreach (var group in groupBySportTypes)
            {
                var sportTypeName = CacheRefServiceHelper.GetSportName(group.Key);
                if (sportTypeName.IndexOf(IgnoreSportTypeName, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    yield return new Sport(
                        group.Key,
                        sportTypeName,
                        BuildLeagues(group.Key, group).ToList());
                }
            }
        }

        private static IEnumerable<League> BuildLeagues(int sportType, IEnumerable<League> leagueList)
            => leagueList.GroupBy(
                l => l.LeagueId,
                (leagueId, matches) => new League(
                    sportType,
                    leagueId,
                    leagueId == 0 ? Resources.AllLeagues : CacheRefServiceHelper.GetLeagueName(leagueId)));

        public async Task<Result> AddFilter(AddFilterParam param)
        {
            var command = new AddFilterCommand(param);
            partialUMService.ThrowIfUM(command.GetSettingKey());

            await dynamicRepository.ExecuteAsync(command);
            return new Result(
                command.ErrCode,
                command.ErrCode == 0 ? Resources.AddedSuccessfully : command.ErrMsg);
        }

        public async Task<Result> UpdateFilter(UpdateFilterParam param)
        {
            var command = new UpdateFilterCommand(param);
            partialUMService.ThrowIfUM(command.GetSettingKey());

            await dynamicRepository.ExecuteAsync(command);
            return new Result(
                command.ErrCode,
                command.ErrCode == 0 ? Resources.UpdateSuccessfully : command.ErrMsg);
        }

        public async Task<IEnumerable<Result>> DeleteFilters(DeleteFiltersParam param)
        {
            var results = new List<Result>();
            var filterIdList = param.FilterIds.ToListInt();

            foreach (var filterId in filterIdList)
            {
                var command = new DeleteFilterCommand(param.CustId, param.SubAccId, filterId, param.IP);
                partialUMService.ThrowIfUM(command.GetSettingKey());

                await dynamicRepository.ExecuteAsync(command);
                results.Add(new Result(command.ErrCode, command.ErrMsg));
            }

            return results;
        }

        public async Task<IEnumerable<Sport>> GetSports(byte sportGroup)
        {
            var criteria = new GetLeaguesCriteria();
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var leagues = await dynamicRepository.FetchAsync<League>(criteria);
            if (sportGroup != SportGroupConstants.All)
            {
                leagues = sportGroup == SportGroupConstants.MajorSports
                    ? leagues.Where(m => MajorSports.ContainsKey(m.SportType))
                    : leagues.Where(m => !MajorSports.ContainsKey(m.SportType));
            }

            var sports = BuildSports(leagues.ToList()).ToList();
            return sports;
        }

        public async Task<IEnumerable<MemberErrCode>> CheckUsernames(int agentId, string usernames)
        {
            var criteria = new CheckUsernamesCriteria(agentId, usernames);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var checkUsernames = await dynamicRepository.FetchAsync<MemberErrCode>(criteria);
            foreach (var item in checkUsernames)
            {
                if (item.ErrCode == InvalidUsername)
                {
                    item.ErrMsg = Resources.InvalidUsername;
                }
                else if (item.ErrCode == NotAMember)
                {
                    item.ErrMsg = Resources.NotAMember;
                }
            }

            return checkUsernames;
        }

        public Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId)
            => dynamicRepository.FetchAsync<MyWatchlistMember>(new GetMonitoredWatchlistMembersCriteria(agentId));

        public async Task<IEnumerable<TicketDto>> GetLastFilteringBets(GetLastFilteringBetsParams param)
        {
            var criteria = new GetLastFilteringBetsCriteria(param.CustId, param.TransId,
                param.MemberList, param.SportTypeList,
                param.LeagueList, param.BetTypeList,
                param.LiveIndicator,
                param.MinStake, param.LimitRowCount);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);

            if (tickets.Any())
            {
                var cacheTime = AppSettings.RunningMatchCacheTime;
                var outrightMatches = await lastBetsService.GetRunningOutrightMatches(cacheTime);
                var otherMatches = await lastBetsService.GetRunningOtherMatches(cacheTime);

                foreach (var ticket in tickets)
                {
                    var matches = BetTypeHelper.IsOutRight(ticket.BetTypeId) ? outrightMatches : otherMatches;
                    var matchInfo = lastBetsService.GetMatchInfo(ticket.MatchId, matches, cacheTime);

                    LastBetsServiceImpl.AssignMatchInfoToTicket(ticket, matchInfo);
                }
            }

            return tickets;
        }
    }
}
