namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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

        public WinLossByMatchServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(WinLossByMatchParam param)
        {
            Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> betListDataTuple;

            if (BetTypeHelper.IsOutRight(param.BetType))
            {
                betListDataTuple = await GetWinlossOutright(param);
            }
            else if (BetTypeHelper.IsHorseRacing(param.BetType))
            {
                betListDataTuple = await GetWinlossRacing(param);
            }
            else
            {
                betListDataTuple = await GetWinlossOthers(param);
            }

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetWinlossOutright(WinLossByMatchParam param)
            => await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                new GetWinLossOutrightCriteria(
                    param.CustId,
                    param.LeagueId,
                    param.Language,
                    param.FromDate,
                    param.ToDate,
                    reportDateService.GetMoveBetsDay(),
                    param.CustLevel));

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetWinlossRacing(WinLossByMatchParam param)
            => await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                new GetWinLossRacingCriteria(
                    param.CustId,
                    param.BetType,
                    param.Race,
                    param.FromDate,
                    param.ToDate,
                    reportDateService.GetMoveBetsDay(),
                    param.CustLevel));

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetWinlossOthers(WinLossByMatchParam param)
            => await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                new GetWinLossByMatchCriteria(
                    param.CustId,
                    param.MatchId,
                    param.BetType,
                    param.CustName,
                    param.LiveIndicator,
                    param.CustLevel));
    }
}
