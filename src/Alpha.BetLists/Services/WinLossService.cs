namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria.WinLossByDate;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Example.MVC.Services;
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

        public BetListData GetDataSource()
        {
            var model = new Fanex.BetList.Example.MVC.ViewModel.BetListData();
            Fanex.BetList.Example.MVC.ViewModel.BetListData betListData = (new DataService()).GetBetListData("~/App_Data/BetListDummyData1.xls", model.BetTypeId);

            // check whether request filter by bet type id, and does not contain select all
            if (model.BetTypeId != 0)
            {
                model.Tickets = betListData
                    .Tickets
                    .Where(ticket => model.BetTypeId == ticket.BetTypeId)
                    .Select(ticket => ticket)
                    .ToList();

                model.TicketData = betListData
                  .TicketData
                  .Where(ticketData => model.BetTypeId == ticketData.BetTypeId)
                  .Select(ticket => ticket)
                  .ToList();

                model.TicketData = model.TicketData ?? new List<ITicketData>();
                model.Tickets = model.Tickets ?? new List<ITicket>();
            }
            else
            {
                model.TicketData = betListData.TicketData ?? new List<ITicketData>();
                model.Tickets = betListData.Tickets ?? new List<ITicket>();
            }

            return new BetListData(model.Tickets, model.TicketData);
        }

        public BetListData GetBetList(WinLossParam param)
        {
            return GetDataSource();
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
                    betListDataTuple = GetBetList(param, fromDate: beforeBalanceUpDate);
                    var oldBetListDataTuple = GetBetListOld(param, toDate: beforeBalanceUpDate);

                    return new BetListData(
                        betListDataTuple.Item1.Concat(oldBetListDataTuple.Item1),
                        betListDataTuple.Item2.Concat(oldBetListDataTuple.Item2).ToList<ITicketData>());
                }
            }
            else
            {
                betListDataTuple = GetBetList(param, param.FromDate);
            }

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> GetBetList(WinLossParam param, DateTime fromDate)
        {
            return dynamicRepository.FetchMultiple<Ticket, TicketData>(
                        new GetBetListWinLossByDateCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            fromDate,
                            param.ToDate,
                            reportDateService.GetMoveBetsDay(),
                            param.Language));
        }

        private Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> GetBetListOld(WinLossParam param, DateTime toDate)
        {
            return dynamicRepository.FetchMultiple<Ticket, TicketData>(
                        new GetBetListWinLossByDateOldCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            param.FromDate,
                            toDate,
                            param.Language));
        }
    }
}