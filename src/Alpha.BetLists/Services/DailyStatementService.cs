namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
    using AlphaPlus.PartialUM.Services;
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
        private readonly IBetListRepository betlistRepository;

        public DailyStatementServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            IBetListRepository betlistRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.betlistRepository = betlistRepository;
        }

        public async Task<BetListData> GetBetList(DailyStatementParam param)
        {
            var balanceUpDate = reportDateService.GetBalanceUpDay();

            var betListDataTuple = param.WinLossDate.IsBefore(balanceUpDate)
                ? await GetBetListByDayOld(param)
                : await GetBetListByDay(param);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        private async Task<(IEnumerable<TicketDto>, IEnumerable<TicketData>)> GetBetListByDayOld(DailyStatementParam param)
        {
            var criteria = new GetAllBetListByDayOldCriteria(
                param.CustId,
                param.WinLossDate,
                param.TransType,
                param.Language);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            //return betlistRepository.FetchMultiple(criteria);
        }

        private async Task<(IEnumerable<TicketDto>, IEnumerable<TicketData>)> GetBetListByDay(DailyStatementParam param)
        {
            var criteria = new GetAllBetListByDayCriteria(
                param.CustId,
                param.WinLossDate,
                reportDateService.GetMoveBetsDay(),
                param.TransType);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            //return betlistRepository.FetchMultiple(criteria);
        }
    }
}
