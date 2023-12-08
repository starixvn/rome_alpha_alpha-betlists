namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.CancelledBet;
    using Alpha.BetLists.DataAccesses;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using AlphaPlus.PartialUM.Services;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;

    public interface CancelledBetService
    {
        Task<PagedReportData<BetListData>> GetCancelledBet(CancelledBetParam parameter);
    }

    public class CancelledBetServiceImpl : CancelledBetService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private readonly IBetListRepository betlistRepository;

        public CancelledBetServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.betlistRepository = betlistRepository;
        }

        public async Task<PagedReportData<BetListData>> GetCancelledBet(CancelledBetParam parameter)
        {
            var criteria = new GetCancelledBetCriteria(
                parameter.CustId,
                parameter.FromDate,
                parameter.ToDate,
                parameter.MoveBetsDay,
                parameter.RoleId,
                parameter.ProductIds.JoinWithComma(),
                parameter.StatusIds,
                parameter.PageSize,
                parameter.PageIndex);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            //var betListDataTuple = betlistRepository.FetchMultiple(criteria);

            return new PagedReportData<BetListData>(
                new List<BetListData>
                {
                    new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>())
                },
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRows);
        }
    }
}
