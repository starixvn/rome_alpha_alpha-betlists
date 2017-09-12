namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.BetDetail;
    using Fanex.Data.Repository;
    using Models;
    using Params.BetDetail;

    public interface BetDetailService
    {
        Task<IEnumerable<TicketDto>> GetRNGCasinoDetails(BetDetailParam param);

        Task<IEnumerable<TicketDto>> GetAllbetDetails(BetDetailParam param);
    }

    public class BetDetailServiceImpl : BetDetailService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BetDetailServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetAllbetDetails(BetDetailParam param)
        {
            var criteria = new GetAllbetBetDetailCriteria(param.RefNo, param.CustId, param.TransType, param.PageIndex, param.PageSize);

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            param.TotalRows = criteria.TotalRows;

            return tickets;
        }

        public async Task<IEnumerable<TicketDto>> GetRNGCasinoDetails(BetDetailParam param)
        {
            var criteria = new GetRNGBetDetailCriteria(param.RefNo, param.CustId, param.TransType, param.PageIndex, param.PageSize);

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            param.TotalRows = criteria.TotalRows;

            return tickets;
        }
    }
}
