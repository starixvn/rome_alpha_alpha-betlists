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
        Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId, AddFilterParam customFilter);

        Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId, AddFilterParam customFilter);

        Task<Result> AddFilter(AddFilterParam param);

        Task<Result> UpdateFilter(UpdateFilterParam param);

        Task<Result> DeleteFilter(DeleteFilterParam param);

        Task<IEnumerable<Sport>> GetSports(byte sportGroup);

        Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId);

        Task<IEnumerable<TicketDto>> GetLastFilteringBets(GetLastFilteringBetsParams param);
    }

    public class MonitoringBetsServiceImpl : MonitoringBetsService
    {
        private const string IgnoreSportTypeName = "upcoming";
        private static readonly HashSet<int> MajorSports = new HashSet<int>
        {
            SportTypeIdConstants.Soccer,
            SportTypeIdConstants.Basketball,
            SportTypeIdConstants.Badminton,
            SportTypeIdConstants.ESports,
            SportTypeIdConstants.Tennis
        };
        private static readonly HashSet<string> SpecialFilterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Unsaved Filter"
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

        public async Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId, AddFilterParam customFilter)
        {
            var filterConfiguration = (await GetFilterConfiguration(custId, subAccId))
                .Select(f =>
                {
                    var filterXml = f.FilterXml;
                    var (sportTypes, leagues) = BuildSportTypesAndLeagues(filterXml.Sports);

                    return new SavedFilter(
                        f.FilterId,
                        f.FilterName,
                        f.IsDefault,
                        filterXml.BetType,
                        filterXml.Stake,
                        filterXml.LiveStatus,
                        sportTypes,
                        leagues,
                        BuildMembers(filterXml.FilterByMembers, filterXml.Members, filterXml.MyWatchlistMembers),
                        filterXml.FilterByMembers);
                })
                .ToList();

            if (customFilter != null)
            {
                filterConfiguration.Insert(0, BuildSavedFilter(customFilter));
            }

            if (filterConfiguration.Count > 0 && !filterConfiguration.Any(f => f.IsDefault))
            {
                filterConfiguration.Insert(0, new SavedFilter());
            }

            return filterConfiguration;
        }

        private static SavedFilter BuildSavedFilter(AddFilterParam customFilter)
        {
            var (sportTypes, leagues) = BuildSportTypesAndLeagues(customFilter.Sports);

            return new SavedFilter(
                0,
                Resources.UnsavedFilter,
                false,
                customFilter.BetType,
                customFilter.Stake,
                customFilter.LiveStatus,
                sportTypes,
                leagues,
                BuildMembers(customFilter.FilterByMembers, customFilter.InputList, customFilter.MyWatchlist),
                customFilter.FilterByMembers);
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

            return $"{members},{myWatchlistMembers}".Trim(',');
        }

        public async Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId, AddFilterParam customFilter)
        {
            var filterConfiguration = (await GetFilterConfiguration(custId, subAccId))
                .Select(item =>
                {
                    var filterXml = item.FilterXml;
                    filterXml.BetTypeNames = BuildBetTypeNames(filterXml.BetType);
                    filterXml.FlatternSports = BuildSports(FlattenSports(filterXml.Sports)).ToList();

                    return item;
                })
                .ToList();

            if (customFilter != null)
            {
                filterConfiguration.Insert(0, BuildFilterConfiguration(customFilter));
            }

            return filterConfiguration;
        }

        private async Task<IEnumerable<FilterConfiguration>> GetFilterConfiguration(int custId, int subAccId)
        {
            var criteria = new GetFiltersCriteria(custId, subAccId);
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return (await dynamicRepository.FetchAsync<FilterConfiguration>(criteria)).OrderByDescending(f => f.FilterId);
        }

        private static FilterConfiguration BuildFilterConfiguration(AddFilterParam param)
        {
            var filterXml = new FilterXml
            {
                BetType = param.BetType,
                Stake = param.Stake,
                Members = param.InputList,
                MyWatchlistMembers = param.MyWatchlist,
                FilterByMembers = param.FilterByMembers,
                LiveStatus = param.LiveStatus,
                Sports = param.Sports.ToList()
            };

            filterXml.BetTypeNames = BuildBetTypeNames(filterXml.BetType);
            filterXml.FlatternSports = BuildSports(FlattenSports(filterXml.Sports)).ToList();

            return new FilterConfiguration(
                param.FilterId,
                param.FilterName,
                string.Empty,
                filterXml,
                param.IsDefault,
                DateTime.MinValue,
                param.MyWatchlistMembers,
                param.InputListMembers);
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
            if (SpecialFilterNames.Contains(param.FilterName))
            {
                const int AlreadyExistsErrorCode = 90020;
                return new Result(AlreadyExistsErrorCode, Resources.FilterNameAlreadyExists);
            }

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

        public async Task<Result> DeleteFilter(DeleteFilterParam param)
        {
            var command = new DeleteFilterCommand(param);
            partialUMService.ThrowIfUM(command.GetSettingKey());

            await dynamicRepository.ExecuteAsync(command);

            return new Result(command.ErrCode, command.ErrMsg);
        }

        public async Task<IEnumerable<Sport>> GetSports(byte sportGroup)
        {
            var criteria = new GetLeaguesCriteria();
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var leagues = await dynamicRepository.FetchAsync<League>(criteria);
            if (sportGroup != SportGroupConstants.All)
            {
                leagues = sportGroup == SportGroupConstants.MajorSports
                    ? leagues.Where(m => MajorSports.Contains(m.SportType))
                    : leagues.Where(m => !MajorSports.Contains(m.SportType));
            }

            var sports = BuildSports(leagues.ToList()).ToList();
            return sports;
        }

        public Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId)
            => dynamicRepository.FetchAsync<MyWatchlistMember>(new GetMonitoredWatchlistMembersCriteria(agentId));

        public Task<IEnumerable<TicketDto>> GetLastFilteringBets(GetLastFilteringBetsParams param)
            => lastBetsService.GetLastBets(
                new GetLastFilteringBetsCriteria(
                    param.CustId,
                    param.TransId,
                    param.MemberList,
                    param.SportTypeList,
                    param.LeagueList,
                    param.BetTypeList,
                    param.LiveIndicator,
                    param.MinStake,
                    param.LimitRowCount),
                AppSettings.RunningMatchCacheTime);
    }
}
