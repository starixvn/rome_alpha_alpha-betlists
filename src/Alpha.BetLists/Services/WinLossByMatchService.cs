namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AlphaPlus.PartialUM.Services;
    using Criteria.WinLossByMatch;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Params;

    public interface WinLossByMatchService
    {
        Task<BetListData> GetBetList(WinLossByMatchParam param);
    }

    public class WinLossByMatchServiceImpl : WinLossByMatchService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public WinLossByMatchServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<BetListData> GetBetList(WinLossByMatchParam param)
        {
            (IEnumerable<TicketDto>, IEnumerable<TicketData>) betListDataTuple;

            if (BetTypeHelper.IsOutRight(param.BetType))
            {
                betListDataTuple = await GetWinlossOutright(param);
            }
            else if (BetTypeHelper.IsHorseRacing(param.BetType))
            {
                betListDataTuple = await GetWinlossRacing(param);
            }
            else if (BetTypeHelper.IsLeapGaming(param.BetType) || BetTypeHelper.IsHappy5LinkedGames(param.BetType))
            {
                betListDataTuple = await GetWinlossByBettype(param);
            }
            else
            {
                betListDataTuple = await GetWinlossOthers(param);
            }

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private async Task<(IEnumerable<TicketDto>, IEnumerable<TicketData>)> GetWinlossOutright(WinLossByMatchParam param)
        {
            var criteria = new GetWinLossOutrightCriteria(
                param.CustId,
                param.LeagueId,
                param.Language,
                param.FromDate,
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        }

        private async Task<(IEnumerable<TicketDto>, IEnumerable<TicketData>)> GetWinlossRacing(WinLossByMatchParam param)
        {
            var criteria = new GetWinLossRacingCriteria(
                param.CustId,
                param.BetType,
                param.Race,
                param.FromDate,
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        }

        private async Task<(IEnumerable<TicketDto>, IEnumerable<TicketData>)> GetWinlossOthers(WinLossByMatchParam param)
        {
            var criteria = new GetWinLossByMatchCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                param.MatchId,
                param.BetType,
                param.CustName,
                param.LiveIndicator,
                param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        }

        private async Task<(IEnumerable<TicketDto>, IEnumerable<TicketData>)> GetWinlossByBettype(WinLossByMatchParam param)
        {
            var criteria = new GetWinLossByBetTypeCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDateService.GetMoveBetsDay(),
                param.CustLevel,
                param.BetType);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        }
    }
}
