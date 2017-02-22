namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLossByDate;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface WinLossService
    {
        BetListData GetBetList(WinLossParam param);
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

        public BetListData GetBetList(WinLossParam param)
        {
            var balanceUpDate = reportDateService.GetBalanceUpDay();
            var beforeBalanceUpDate = balanceUpDate.AddDays(-1);
            Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> betListDataTuple;

            if (param.FromDate.IsBefore(balanceUpDate))
            {
                if (param.ToDate.IsBefore(beforeBalanceUpDate))
                {
                    betListDataTuple = GetBetListOld(param, param.ToDate);
                }
                else
                {
                    betListDataTuple = GetBetList(param, beforeBalanceUpDate);
                    var oldBetListDataTuple = GetBetListOld(param, beforeBalanceUpDate);

                    return new BetListData(
                        betListDataTuple.Item1.Concat(oldBetListDataTuple.Item1),
                        betListDataTuple.Item2.Concat(oldBetListDataTuple.Item2));
                }
            }
            else
            {
                betListDataTuple = GetBetList(param, param.FromDate);
            }

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2);
        }

        private Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> GetBetList(WinLossParam param, DateTime fromDate)
        {
            return dynamicRepository.FetchMultiple<Ticket, TicketData>(
                        new GetBetListWinLossByDateCriteria(param.CustId, param.ProductIds.JoinWithComma(), fromDate, param.ToDate, reportDateService.GetMoveBetsDay(), param.Language));
        }

        private Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> GetBetListOld(WinLossParam param, DateTime toDate)
        {
            return dynamicRepository.FetchMultiple<Ticket, TicketData>(
                        new GetBetListWinLossByDateOldCriteria(param.CustId, param.ProductIds.JoinWithComma(), param.FromDate, toDate, param.Language));
        }
    }
}