namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.PartialUM.Services;
    using Criteria.DailyStatement;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface DailyStatementService
    {
        Task<BetListData> GetBetList(DailyStatementParam param);
    }

    public class DailyStatementServiceImpl : DailyStatementService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public DailyStatementServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<BetListData> GetBetList(DailyStatementParam param)
        {
            var balanceUpDate = reportDateService.GetBalanceUpDay();
            Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> betListDataTuple;

            betListDataTuple = param.WinLossDate.IsBefore(balanceUpDate)
                ? await GetBetListByDayOld(param)
                : await GetBetListByDay(param);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetBetListByDayOld(DailyStatementParam param)
        {
            var criteria = new GetAllBetListByDayOldCriteria(
                param.CustId,
                param.WinLossDate,
                param.TransType,
                param.Language);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        }

        private async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>>> GetBetListByDay(DailyStatementParam param)
        {
            var criteria = new GetAllBetListByDayCriteria(
                param.CustId,
                param.WinLossDate,
                reportDateService.GetMoveBetsDay(),
                param.TransType);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        }
    }
}
