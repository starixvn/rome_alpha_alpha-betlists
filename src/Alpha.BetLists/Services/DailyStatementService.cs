namespace Alpha.BetLists.Services
{
    using Criteria.DailyStatement;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface DailyStatementService
    {
        Task<BetListData> GetBetList(DailyStatementParam param);
    }

    public class DailyStatementServiceImpl : DailyStatementService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;

        public DailyStatementServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(DailyStatementParam param)
        {
            var balanceUpDate = reportDateService.GetBalanceUpDay();
            Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> betListDataTuple;

            betListDataTuple = param.WinLossDate.IsBefore(balanceUpDate)
                ? await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                    new GetAllBetListByDayOldCriteria(
                        param.CustId,
                        param.WinLossDate,
                        param.TransType,
                        param.Language))
                : await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(
                    new GetAllBetListByDayCriteria(
                        param.CustId,
                        param.WinLossDate,
                        reportDateService.GetMoveBetsDay(),
                        param.TransType,
                        param.Language));

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}
