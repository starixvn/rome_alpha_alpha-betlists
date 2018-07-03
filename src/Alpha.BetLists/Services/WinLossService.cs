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
            var previousBalanceUpDate = balanceUpDate.AddDays(-1);
            Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> betListDataTuple;
            var fromDate = param.FromDate.GetGreaterDate(param.LimitedDate);

            if (fromDate.IsBefore(balanceUpDate))
            {
                if (param.ToDate.IsBeforeOrEqual(previousBalanceUpDate))
                {
                    betListDataTuple = await GetBetListBeforeBalanceUpDay(param, fromDate: fromDate, toDate: param.ToDate);
                }
                else
                {
                    betListDataTuple = await GetBetList(param, fromDate: balanceUpDate);
                    var oldBetListDataTuple = await GetBetListBeforeBalanceUpDay(param, fromDate: fromDate, toDate: previousBalanceUpDate);

                    return new BetListData(
                        betListDataTuple.Item1.Concat(oldBetListDataTuple.Item1),
                        betListDataTuple.Item2.Concat(oldBetListDataTuple.Item2).ToList<ITicketData>());
                }
            }
            else
            {
                betListDataTuple = await GetBetList(param, fromDate);
            }

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetBetList(WinLossParam param, DateTime fromDate)
            => await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                        new GetBetListWinLossByDateCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            fromDate,
                            param.ToDate,
                            reportDateService.GetMoveBetsDay(),
                            param.Language));

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetBetListBeforeBalanceUpDay(WinLossParam param, DateTime fromDate, DateTime toDate)
            => await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                        new GetBetListWinLossByDateOldCriteria(
                            param.CustId,
                            param.ProductIds.JoinWithComma(),
                            fromDate,
                            toDate,
                            param.Language));
    }
}