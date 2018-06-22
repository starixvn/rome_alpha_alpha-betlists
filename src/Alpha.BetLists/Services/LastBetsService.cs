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

            if (tickets.Any())
            {
                var cacheTime = param.RunningMatchCacheTime;
                var outrightMatches = await GetRunningOutrightMatches(cacheTime);
                var otherMatches = await GetRunningOtherMatches(cacheTime);

                foreach (var ticket in tickets)
                {
                    if (BetTypeHelper.IsNotParlay(ticket.BetTypeId))
                    {
                        var matches = BetTypeHelper.IsOutRight(ticket.BetTypeId) ? outrightMatches : otherMatches;
                        var matchInfo = GetMatchInfo(ticket.MatchId, matches);

                        AssignMatchInfoToTicket(ticket, matchInfo);
                    }
                }
            }

            return tickets;
        }

        public bool CanViewLastBets(CheckCanViewLastBetsParam param)
            => param.IsInternal
            || param.IsEnableLastBetsMonitoring
            || param.UsersAlwaysAllowAccessBetsListMonitoring.Contains(param.CustId);

        private static bool IsDummyMatch(int matchId) => matchId <= 999;

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

        private MatchInfo GetMatchInfo(int matchId, List<MatchInfo> matches)
        {
            var matchInfo = matches.FirstOrDefault(match => match.MatchId == matchId);

            if (matchInfo == null && !IsDummyMatch(matchId))
            {
                matchInfo = dynamicRepository.Fetch<MatchInfo>(new GetMatchInfoByIdCriteria(matchId)).FirstOrDefault();

                if (matchInfo != null)
                {
                    matches.Add(matchInfo);
                }
            }

            return matchInfo;
        }

        private async Task<List<MatchInfo>> GetRunningOutrightMatches(int cacheTime)
        {
            var cacheKey = RunningOutrightMatchesCacheKey;

            var matches = await ConfiguredCacheService.GetOrSetAsync(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOutrightMatchCriteria()).ToList(),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches;
        }

        private async Task<List<MatchInfo>> GetRunningOtherMatches(int cacheTime)
        {
            var cacheKey = RunningOtherMatchesCacheKey;

            var matches = await ConfiguredCacheService.GetOrSetAsync(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOtherMatchCriteria()).ToList(),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches;
        }
    }
}