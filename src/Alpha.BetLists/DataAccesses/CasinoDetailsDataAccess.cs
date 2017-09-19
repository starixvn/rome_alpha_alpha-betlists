namespace Alpha.BetLists.DataAccesses
{
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

        public CasinoDetailsDataAccess(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public PagedData<ITicket> GetCasinoDetails(CasinoDetailsParam param)
        {
            var criteria = new GetCasinoDetailsCriteria(
                param.RefNo,
                param.CustId,
                param.TransType,
                param.ProductId,
                param.PageIndex,
                param.PageSize);

            var tickets = dynamicRepository.Fetch<TicketDto>(criteria);

            return new PagedData<ITicket>(tickets, criteria.PageIndex, criteria.PageSize, criteria.TotalRows);
        }
    }
}
