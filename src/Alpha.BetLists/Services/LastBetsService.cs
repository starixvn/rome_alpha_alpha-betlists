namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.LastBets;
    using Fanex.BetList.Core.Entities;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Params.LastBets;

    public interface LastBetsService
    {
        Task<IEnumerable<TicketDto>> GetLastBets(GetLastBetsParam param);

        bool CanViewLastBets(CheckCanViewLastBetsParam param);
    }

    public class LastBetsServiceImpl : LastBetsService
    {
        private const string RunningOutrightMatchesCacheKey = "Alpha.Reports.Services.Impl.LastBetsServiceImpl.RunningOutrightMatchesCacheKey";
        private const string RunningOtherMatchesCacheKey = "Alpha.Reports.Services.Impl.LastBetsServiceImpl.RunningOtherMatchesCacheKey";

        private readonly IDynamicRepository dynamicRepository;

        public LastBetsServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetLastBets(GetLastBetsParam param)
        {
            var tickets = await dynamicRepository.FetchAsync<TicketDto>(
                new GetLastBetsCriteria(param.BetType, param.CustId, param.TransId, param.PageSize));

            foreach (var ticket in tickets)
            {
                var matches = GetRunningMatches(ticket.BetTypeId, getNewData: false);
                var matchInfo = GetMatchInfo(ticket, matches);

                AssignMatchInfoToTicket(ticket, matchInfo);
            }

            return tickets;
        }

        public bool CanViewLastBets(CheckCanViewLastBetsParam param)
            => param.IsInternal
            || param.IsEnableLastBetsMonitoring
            || param.UsersAlwaysAllowAccessBetsListMonitoring.Contains(param.CustId);

        private static bool IsDummyMatch(int matchId) => matchId <= 999;

        private static MatchInfo FilterMatch(int matchId, IEnumerable<MatchInfo> matches) =>
            matches != null
            ? matches.FirstOrDefault(match => match.MatchId == matchId)
            : null;

        private static void AssignMatchInfoToTicket(Ticket ticket, MatchInfo matchInfo)
        {
            if (matchInfo == null)
            {
                return;
            }

            ticket.MatchCode = matchInfo.MatchCode;
            ticket.SportTypeId = matchInfo.SportType;
            ticket.LeagueId = matchInfo.LeagueId;
            ticket.HomeId = matchInfo.HomeId;
            ticket.AwayId = matchInfo.AwayId;
            ticket.Race = matchInfo.Race;
            ticket.ShowTime = matchInfo.ShowTime;
            ticket.IsNeutral = matchInfo.IsNeutral;
        }

        private IEnumerable<MatchInfo> GetRunningMatches(int betType, bool getNewData)
        {
            var isOutright = BetTypeHelper.IsOutRight(betType);

            var matches = isOutright
                ? GetRunningOutrightMatches(getNewData)
                : GetRunningOtherMatches(getNewData);

            return matches;
        }

        private IEnumerable<MatchInfo> GetRunningOutrightMatches(bool getNewData)
        {
            var cacheKey = RunningOutrightMatchesCacheKey;

            if (getNewData)
            {
                ConfiguredCacheService.Remove(cacheKey);
            }

            var matches = ConfiguredCacheService.GetOrSet(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOutrightMatchCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));

            return matches;
        }

        private IEnumerable<MatchInfo> GetRunningOtherMatches(bool getNewData)
        {
            var cacheKey = RunningOtherMatchesCacheKey;

            if (getNewData)
            {
                ConfiguredCacheService.Remove(cacheKey);
            }

            var matches = ConfiguredCacheService.GetOrSet(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOtherMatchCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15)));

            return matches;
        }

        private MatchInfo GetMatchInfo(Ticket ticket, IEnumerable<MatchInfo> matches)
        {
            var matchId = ticket.MatchId;

            var matchInfo = FilterMatch(matchId, matches);

            if (matchInfo == null && !IsDummyMatch(matchId))
            {
                var updatedMatches = GetRunningMatches(ticket.BetTypeId, getNewData: true);
                matchInfo = FilterMatch(matchId, updatedMatches);
            }

            return matchInfo;
        }
    }
}
