namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.PartialUM.Services;
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

        MatchInfo GetMatchInfoById(int matchId, int cacheTime);
    }

    public class LastBetsServiceImpl : LastBetsService
    {
        private const string RunningMatchesCacheKey = "Alpha.Reports.Services.Impl.LastBetsServiceImpl.RunningMatchesCacheKey";
        private const string RunningOutrightMatchesCacheKey = "Alpha.Reports.Services.Impl.LastBetsServiceImpl.RunningOutrightMatchesCacheKey";
        private const string RunningOtherMatchesCacheKey = "Alpha.Reports.Services.Impl.LastBetsServiceImpl.RunningOtherMatchesCacheKey";
        private const string CheckRunningMatchByIdCacheKey = "Alpha.Reports.Services.Impl.LastBetsServiceImpl.CheckRunningMatchByIdCacheKey";

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private readonly IPartialUMService partialUMService;

        public LastBetsServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.partialUMService = partialUMService;
        }

        public async Task<IEnumerable<TicketDto>> GetLastBets(GetLastBetsParam param)
        {
            var criteria = new GetLastBetsCriteria(param.BetType, param.CustId, param.TransId, param.PageSize);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);

            if (tickets.Any())
            {
                var cacheTime = param.RunningMatchCacheTime;
                var outrightMatches = await GetRunningOutrightMatches(cacheTime);
                var otherMatches = await GetRunningOtherMatches(cacheTime);

                foreach (var ticket in tickets)
                {
                    var matches = BetTypeHelper.IsOutRight(ticket.BetTypeId) ? outrightMatches : otherMatches;
                    var matchInfo = GetMatchInfo(ticket.MatchId, matches, cacheTime);

                    AssignMatchInfoToTicket(ticket, matchInfo);
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
            ticket.EventStatus = matchInfo.BetStatus;
            ticket.SettlementStatus = matchInfo.SettlementStatus;
            ticket.EventDate = matchInfo.EventDate;
        }

        private MatchInfo GetMatchInfo(int matchId, List<MatchInfo> matches, int cacheTime)
        {
            var cachedKey = $"{RunningMatchesCacheKey}_{matchId}";
            var matchInfo = matches.FirstOrDefault(match => match.MatchId == matchId)
                ?? cacheService.Get<MatchInfo>(cachedKey);

            if (matchInfo == null && !IsDummyMatch(matchId))
            {
                matchInfo = dynamicRepository.Fetch<MatchInfo>(new GetMatchInfoByIdCriteria(matchId))
                    .FirstOrDefault();

                if (matchInfo != null)
                {
                    cacheService.Set(
                        cachedKey,
                        matchInfo,
                        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));
                }
            }

            return matchInfo;
        }

        private async Task<List<MatchInfo>> GetRunningOutrightMatches(int cacheTime)
        {
            var cacheKey = RunningOutrightMatchesCacheKey;

            var matches = await cacheService.GetOrSetAsync(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOutrightMatchCriteria()).ToList(),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches;
        }

        private async Task<List<MatchInfo>> GetRunningOtherMatches(int cacheTime)
        {
            var cacheKey = RunningOtherMatchesCacheKey;

            var matches = await cacheService.GetOrSetAsync(
                cacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetRunningOtherMatchCriteria()).ToList(),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches;
        }

        public MatchInfo GetMatchInfoById(int matchId, int cacheTime)
        {
            var cachedKey = $"{CheckRunningMatchByIdCacheKey}_{matchId}";
            var matchInfo = cacheService.Get<MatchInfo>(cachedKey);

            if (matchInfo == null && !IsDummyMatch(matchId))
            {
                matchInfo = dynamicRepository.Fetch<MatchInfo>(new GetMatchInfoByIdCriteria(matchId)).FirstOrDefault();

                if (matchInfo != null)
                {
                    cacheService.Set(
                        cachedKey,
                        matchInfo,
                        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));
                }
            }

            return matchInfo;
        }
    }
}