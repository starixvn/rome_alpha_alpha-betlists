namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.BetDetail;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface RNGBetDetailService
    {
        Task<IEnumerable<TicketDto>> GetBetDetails(RNGBetDetailParam param);
    }

    public class RNGBetDetailServiceImpl : RNGBetDetailService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RNGBetDetailServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetDetails(RNGBetDetailParam param)
        {
            var criteria = new GetRNGBetDetailCriteria(param.RefNo, param.CustId, param.TransType, param.PageIndex, param.PageSize);

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            param.TotalRows = criteria.TotalRows;

            return tickets;
        }
    }
}
