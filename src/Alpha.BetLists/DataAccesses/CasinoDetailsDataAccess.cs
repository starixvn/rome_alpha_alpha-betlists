namespace Alpha.BetLists.DataAccesses
{
    using Alpha.BetLists.Services;
    using Criteria.CasinoDetails;
    using Fanex.BetDetails.Casino.DataAccesses;
    using Fanex.BetDetails.Casino.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;

    public class CasinoDetailsDataAccess : ICasinoDetailsDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly IBetListRepository betlistRepository;

        public CasinoDetailsDataAccess(IDynamicRepository dynamicRepository, ReportDateService reportDateService, IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.betlistRepository = betlistRepository;
        }

        public PagedData<ITicket> GetCasinoDetails(CasinoDetailsParam param)
        {
            var criteria = new GetCasinoDetailsCriteria(
                param.RefNo,
                param.CustId,
                param.TransType,
                param.ProductId,
                param.PageIndex,
                param.PageSize,
                reportDateService.GetMoveBetsDay(),
                reportDateService.GetBalanceUpDay());

            var tickets = dynamicRepository.Fetch<TicketDto>(criteria);
            //var tickets = betlistRepository.Fetch(criteria);
            return new PagedData<ITicket>(tickets, criteria.PageIndex, criteria.PageSize, criteria.TotalRows);
        }
    }
}
