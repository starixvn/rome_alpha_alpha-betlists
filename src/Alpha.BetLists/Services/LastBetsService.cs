namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
    using AlphaPlus.PartialUM.Services;
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

        Task<IEnumerable<TicketDto>> GetLastBets(ICriteria criteria, int cacheTime);

        bool CanViewLastBets(CheckCanViewLastBetsParam param);

        MatchInfo GetMatchInfoById(int matchId, int cacheTime);
        
        MatchInfo GetMatchInfo(int matchId, List<MatchInfo> matches, int cacheTime);
        
        Task<List<MatchInfo>> GetRunningOutrightMatches(int cacheTime);
        
        Task<List<MatchInfo>> GetRunningOtherMatches(int cacheTime);
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
        private readonly ParlayService parlayService;
        private readonly IBetListRepository betlistRepository;

        public LastBetsServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            IPartialUMService partialUMService,
            ParlayService parlayService,
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.partialUMService = partialUMService;
            this.parlayService = parlayService;
            this.betlistRepository = betlistRepository;
        }

        public Task<IEnumerable<TicketDto>> GetLastBets(GetLastBetsParam param)
            => GetLastBets(
                new GetLastBetsCriteria(param.BetType, param.CustId, param.TransId, param.PageSize),
                param.RunningMatchCacheTime);

        public async Task<IEnumerable<TicketDto>> GetLastBets(ICriteria criteria, int cacheTime)
        {
            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            //var tickets = betlistRepository.Fetch(criteria);
            if (tickets.Any())
            {
                var outrightMatchesTask = GetRunningOutrightMatches(cacheTime);
                var otherMatchesTask = GetRunningOtherMatches(cacheTime);
                await Task.WhenAll(outrightMatchesTask, otherMatchesTask);

                var outrightMatches = await outrightMatchesTask;
                var otherMatches = await otherMatchesTask;

                foreach (var ticket in tickets)
                {
                    var matches = BetTypeHelper.IsOutRight(ticket.BetTypeId) ? outrightMatches : otherMatches;
                    var matchInfo = GetMatchInfo(ticket.MatchId, matches, cacheTime);

                    AssignMatchInfoToTicket(ticket, matchInfo);
                }

                await parlayService.UpdateOddsMixParlay(tickets);
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
            ticket.LeagueGroupId = matchInfo.LeagueGroupId;
            ticket.HomeId = matchInfo.HomeId;
            ticket.AwayId = matchInfo.AwayId;
            ticket.Race = matchInfo.Race;
            ticket.ShowTime = matchInfo.ShowTime;
            ticket.IsNeutral = matchInfo.IsNeutral;
            ticket.EventStatus = matchInfo.BetStatus;
            ticket.SettlementStatus = matchInfo.SettlementStatus;
            ticket.EventDate = matchInfo.EventDate;
        }

        public MatchInfo GetMatchInfo(int matchId, List<MatchInfo> matches, int cacheTime)
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

        public async Task<List<MatchInfo>> GetRunningOutrightMatches(int cacheTime)
        {
            var cacheKey = RunningOutrightMatchesCacheKey;

            var matches = await cacheService.GetOrSetAsync(
                cacheKey,
                () => dynamicRepository.FetchAsync<MatchInfo>(new GetRunningOutrightMatchCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches.ToList();
        }

        public async Task<List<MatchInfo>> GetRunningOtherMatches(int cacheTime)
        {
            var cacheKey = RunningOtherMatchesCacheKey;

            var matches = await cacheService.GetOrSetAsync(
                cacheKey,
                () => dynamicRepository.FetchAsync<MatchInfo>(new GetRunningOtherMatchCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime)));

            return matches.ToList();
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