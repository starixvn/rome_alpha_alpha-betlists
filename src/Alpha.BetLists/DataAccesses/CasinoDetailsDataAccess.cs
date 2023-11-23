namespace Alpha.BetLists.DataAccesses
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Services;
    using Criteria.CasinoDetails;
    using Fanex.BetDetails.Casino.DataAccesses;
    using Fanex.BetDetails.Casino.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;

    public class CasinoDetailsDataAccess : ICasinoDetailsDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;

        public CasinoDetailsDataAccess(IDynamicRepository dynamicRepository, ReportDateService reportDateService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
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
            return new PagedData<ITicket>(tickets, criteria.PageIndex, criteria.PageSize, criteria.TotalRows);
        }
    }
}
