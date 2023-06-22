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
        Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId);

        Task<Result> AddFilter(AddFilterParam param);

        Task<Result> UpdateFilter(UpdateFilterParam param);

        Task<IEnumerable<Result>> DeleteFilters(DeleteFiltersParam param);

        Task<IEnumerable<Sport>> GetSports(byte groupOfSports);

        Task<IEnumerable<Sport>> GetSports(bool isLive, byte groupOfSports);

        Task<IEnumerable<MemberErrCode>> CheckUsernames(int agentId, string usernames);

        Task<IEnumerable<MonitoredWatchlistMember>> GetMonitoredWatchlistMembers(int agentId);
    }

    public class MonitoringBetsServiceImpl : MonitoringBetsService
    {
        private static readonly IDictionary<int, bool> MajorSports = new Dictionary<int, bool>
        {
            [1] = true, // Soccer
            [2] = true, // Basketball
            [9] = true, // Badminton
            [43] = true, // E-Sports
            [5] = true // Tennis
        };
        private readonly IDynamicRepository dynamicRepository;

        public MonitoringBetsServiceImpl(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public async Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId)
        {
            var filterConfigurations = await dynamicRepository.FetchAsync<FilterConfiguration>(
                new GetFiltersCriteria(custId, subAccId));

            foreach (var item in filterConfigurations)
            {
                var filterXml = item.FilterXml;
                filterXml.BetTypeName = BuildBetTypes(filterXml.BetType);
                filterXml.FlatternSports = BuildSports(FlattenSports(filterXml.Sports).OrderBy(s => s.SportId)).ToList();
            }

            return filterConfigurations;
        }

        private static string BuildBetTypes(string betTypes)
            => betTypes.Equals("0", StringComparison.OrdinalIgnoreCase)
            ? Resources.All
            : betTypes
                .SplitByCommaWithNoneEmptyEntries()
                .Select(t => CacheRefServiceHelper.GetBetTypeName(Convert.ToInt32(t)))
                .JoinWithComma(", ");

        private static IEnumerable<Match> FlattenSports(IEnumerable<SportXml> sportXmlList)
        {
            foreach (var item in sportXmlList)
            {
                foreach (var sport in item.Sport.SplitByCommaWithNoneEmptyEntries())
                {
                    foreach (var league in item.League.SplitByCommaWithNoneEmptyEntries())
                    {
                        yield return new Match(
                            Convert.ToInt32(sport),
                            Convert.ToInt32(league),
                            item.Match,
                            item.Home,
                            item.Away,
                            item.IsNeutral);
                    }
                }
            }
        }

        private static IEnumerable<Sport> BuildSports(IEnumerable<Match> matchList)
            => matchList.Where(m => m.SportId != 0).GroupBy(
                m => m.SportId,
                (sportId, matches) => new Sport(
                    sportId,
                    CacheRefServiceHelper.GetSportName(sportId),
                    BuildLeagues(matches).ToList()));

        private static IEnumerable<League> BuildLeagues(IEnumerable<Match> matchList)
            => matchList.Where(m => m.LeagueId != 0).GroupBy(
                m => m.LeagueId,
                (leagueId, matches) => new League(
                    leagueId,
                    CacheRefServiceHelper.GetLeagueName(leagueId),
                    Enumerable.Empty<Match>()
                    /*BuildMatches(matches).ToList()*/));

        private static IEnumerable<Match> BuildMatches(IEnumerable<Match> matchList)
            => matchList.Where(m => m.MatchId != 0).Select(match =>
            {
                match.MatchName = string.Format(
                    "{0} - {1}",
                    CacheRefServiceHelper.GetTeamName(match.HomeId),
                    CacheRefServiceHelper.GetTeamName(match.AwayId));

                return match;
            });

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

            foreach (var filterId in param.FilterIds)
            {
                var command = new DeleteFilterCommand(param.CustId, param.SubAccId, filterId, param.IP);
                await dynamicRepository.ExecuteAsync(command);

                results.Add(new Result(command.ErrCode, command.ErrMsg));
            }

            return results;
        }

        public async Task<IEnumerable<Sport>> GetSports(byte groupOfSports)
        {
            var sports = await dynamicRepository.FetchAsync<Sport>(new GetSportsCriteria());

            if (groupOfSports == GroupOfSportsConstants.MajorSports)
            {
                sports = sports.Where(s => MajorSports.ContainsKey(s.Id));
            }
            else if (groupOfSports == GroupOfSportsConstants.MinorSports)
            {
                sports = sports.Where(s => !MajorSports.ContainsKey(s.Id));
            }

            return sports;
        }

        public async Task<IEnumerable<Sport>> GetSports(bool isLive, byte groupOfSports)
        {
            var matches = await dynamicRepository.FetchAsync<Match>(new GetMatchesCriteria());

            //const int NumberTest = 500;
            //if (matches.Count() > NumberTest)
            //{
            //    matches = matches.Take(NumberTest);
            //}

            var currentTime = DateTime.Now;
            matches = isLive
                ? matches.Where(m => m.KickOffTime > currentTime)
                : matches.Where(m => m.KickOffTime <= currentTime);

            if (groupOfSports != GroupOfSportsConstants.All)
            {
                matches = groupOfSports == GroupOfSportsConstants.MajorSports
                    ? matches.Where(m => MajorSports.ContainsKey(m.SportId))
                    : matches.Where(m => !MajorSports.ContainsKey(m.SportId));
            }

            var sports = BuildSports(matches.ToList()).ToList();
            return sports;
        }

        public async Task<IEnumerable<MemberErrCode>> CheckUsernames(int agentId, string usernames)
        {
            const int InvalidUsername = 100;
            const int NotAMember = 101;

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

        public Task<IEnumerable<MonitoredWatchlistMember>> GetMonitoredWatchlistMembers(int agentId)
            => dynamicRepository.FetchAsync<MonitoredWatchlistMember>(new GetMonitoredWatchlistMembersCriteria(agentId));
    }
}
