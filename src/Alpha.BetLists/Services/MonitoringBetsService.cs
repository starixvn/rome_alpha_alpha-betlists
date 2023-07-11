namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Helpers;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Alpha.Web.Models;
    using Fanex.Data.Repository;

    public interface MonitoringBetsService
    {
        Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId);

        Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId);

        Task<Result> AddFilter(AddFilterParam param);

        Task<Result> UpdateFilter(UpdateFilterParam param);

        Task<IEnumerable<Result>> DeleteFilters(DeleteFiltersParam param);

        Task<IEnumerable<Sport>> GetSports(byte sportGroup);

        Task<IEnumerable<MemberErrCode>> CheckUsernames(int agentId, string usernames);

        Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId);
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
        private readonly IDynamicRepository dynamicRepository;

        public MonitoringBetsServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public async Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId)
        {
            var filterConfigurations = await dynamicRepository.FetchAsync<FilterConfiguration>(
                new GetFiltersCriteria(custId, subAccId));

            return filterConfigurations.Select(f => new SavedFilter(
                f.FilterId,
                f.FilterName,
                f.IsDefault,
                f.FilterXml.BetType,
                f.FilterXml.Stake,
                f.FilterXml.Members));
        }

        public async Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId)
        {
            var filterConfigurations = await dynamicRepository.FetchAsync<FilterConfiguration>(
                new GetFiltersCriteria(custId, subAccId));

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
            : betTypes.ToListInt().Select(t => GetBetTypeName(t)).ToList();

        private static string GetBetTypeName(int betTypeId)
            => betTypeId == BetTypeIdConstants.Others
            ? Resources.Others
            : CacheRefServiceHelper.GetBetTypeName(betTypeId);

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
            await dynamicRepository.ExecuteAsync(command);

            return new Result(
                command.ErrCode,
                command.ErrCode == 0 ? Resources.AddedSuccessfully : command.ErrMsg);
        }

        public async Task<Result> UpdateFilter(UpdateFilterParam param)
        {
            var command = new UpdateFilterCommand(param);
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
                await dynamicRepository.ExecuteAsync(command);

                results.Add(new Result(command.ErrCode, command.ErrMsg));
            }

            return results;
        }

        public async Task<IEnumerable<Sport>> GetSports(byte sportGroup)
        {
            var leagues = await dynamicRepository.FetchAsync<League>(new GetMatchesCriteria());

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
            var checkUsernames = await dynamicRepository.FetchAsync<MemberErrCode>(
                new CheckUsernamesCriteria(agentId, usernames));

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
    }
}
