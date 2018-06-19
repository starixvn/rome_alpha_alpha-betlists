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
        private const string RunningMatchesCacheKey = "Alpha.Reports.Services.Impl.LastBetsServiceImpl.RunningMatchesCacheKey";
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

            var cacheTime = param.RunningMatchCacheTime;
            var outrightMatches = GetRunningOutrightMatches(cacheTime);
            var otherMatches = GetRunningOtherMatches(cacheTime);

            foreach (var ticket in tickets)
            {
                var matches = BetTypeHelper.IsOutRight(ticket.BetTypeId) ? outrightMatches : otherMatches;
                var matchInfo = GetMatchInfo(ticket, matches, cacheTime);

                AssignMatchInfoToTicket(ticket, matchInfo);
            }

            return tickets;
        }

        public bool CanViewLastBets(CheckCanViewLastBetsParam param)
            => param.IsInternal
            || param.IsEnableLastBetsMonitoring
            || param.UsersAlwaysAllowAccessBetsListMonitoring.Contains(param.CustId);

        private static MatchInfo FilterMatch(int matchId, IEnumerable<MatchInfo> matches) =>
            matches?.FirstOrDefault(match => match.MatchId == matchId);

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

        private MatchInfo GetMatchInfo(Ticket ticket, IEnumerable<MatchInfo> matches, int cacheTime)
        {
            var matchId = ticket.MatchId;
            var cachedKey = $"{RunningMatchesCacheKey}_{matchId}";
            var matchInfo = FilterMatch(matchId, matches) ?? ConfiguredCacheService.Get<MatchInfo>(cachedKey);

            if (matchInfo != null)
            {
                return matchInfo;
            }

            matchInfo = dynamicRepository.Fetch<MatchInfo>(new GetMatchInfoByIdCriteria(matchId)).FirstOrDefault();
            if (matchInfo != null)
            {
                ConfiguredCacheService.Set(cachedKey, matchInfo, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));
            }

            return matchInfo;
        }

        private IEnumerable<MatchInfo> GetRunningOutrightMatches(int cacheTime)
        {
            var cacheKey = RunningOutrightMatchesCacheKey;

            var matches = ConfiguredCacheService.GetOrSet(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOutrightMatchCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches;
        }

        private IEnumerable<MatchInfo> GetRunningOtherMatches(int cacheTime)
        {
            var cacheKey = RunningOtherMatchesCacheKey;

            var matches = ConfiguredCacheService.GetOrSet(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOtherMatchCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches;
        }
    }
}
