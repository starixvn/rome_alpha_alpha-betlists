namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.CasinoDetail;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface CasinoDetailService
    {
        Task<IEnumerable<TicketDto>> GetCasinoDetails(CasinoDetailParam param);
    }

    public class CasinoDetailServiceImpl : CasinoDetailService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CasinoDetailServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetCasinoDetails(CasinoDetailParam param)
        {
            var criteria = new GetCasinoDetailCriteria(
                param.RefNo,
                param.CustId,
                param.TransType,
                param.ProductId,
                param.PageIndex,
                param.PageSize);

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            param.TotalRows = criteria.TotalRows;

            return tickets;
        }

    }
}
