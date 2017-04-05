namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.WinLossByDate;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface WinLossService
    {
        Task<BetListData> GetBetList(WinLossParam param);
    }

    public class WinLossServiceImpl : WinLossService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;

        public WinLossServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(WinLossParam param)
        {
            var balanceUpDate = reportDateService.GetBalanceUpDay();
            var beforeBalanceUpDate = balanceUpDate.AddDays(-1);
            Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> betListDataTuple;

            if (param.FromDate.IsBefore(balanceUpDate))
            {
                if (param.ToDate.IsBefore(beforeBalanceUpDate))
                {
                    betListDataTuple = await GetBetListBeforeBalanceUpDay(param, param.ToDate);
                }
                else
                {
                    betListDataTuple = await GetBetList(param, fromDate: beforeBalanceUpDate);
                    var oldBetListDataTuple = await GetBetListBeforeBalanceUpDay(param, toDate: beforeBalanceUpDate);

                    return new BetListData(
                        betListDataTuple.Item1.Concat(oldBetListDataTuple.Item1),
                        betListDataTuple.Item2.Concat(oldBetListDataTuple.Item2).ToList<ITicketData>());
                }
            }
            else
            {
                betListDataTuple = await GetBetList(param, param.FromDate);
            }

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private async Task<Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>>> GetBetList(WinLossParam param, DateTime fromDate)
        {
            return await dynamicRepository.FetchMultipleAsync<Ticket, TicketData>(
                        new GetBetListWinLossByDateCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            fromDate,
                            param.ToDate,
                            reportDateService.GetMoveBetsDay(),
                            param.Language));
        }

        private async Task<Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>>> GetBetListBeforeBalanceUpDay(WinLossParam param, DateTime toDate)
        {
            return await dynamicRepository.FetchMultipleAsync<Ticket, TicketData>(
                        new GetBetListWinLossByDateOldCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            param.FromDate,
                            toDate,
                            param.Language));
        }
    }
}