namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.ProgressiveJackpot;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface JackpotDetailsService
    {
        Task<IEnumerable<TicketDto>> GetJackpotDetails(JackpotDetailsParam param);
    }

    public class JackpotDetailsServiceImpl : JackpotDetailsService
    {
        private readonly IDynamicRepository dynamicRepository;

        public JackpotDetailsServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetJackpotDetails(JackpotDetailsParam param)
            => await dynamicRepository.FetchAsync<TicketDto>(new GetJackpotDetailsCriteria(param.TransId, param.CustId, param.BetType));
    }
}