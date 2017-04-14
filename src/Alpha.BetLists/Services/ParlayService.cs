namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.Parlay;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;

    public interface ParlayService
    {
        Task<IEnumerable<ITicket>> GetMixParlayDetail(long transId, string language);

        Task<IEnumerable<ITicket>> GetCombinationMixParlayDetail(long transId, string language);
    }

    public class ParlayServiceImpl : ParlayService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ParlayServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<ITicket>> GetMixParlayDetail(long transId, string language)
        {
            var tickets = await dynamicRepository.FetchAsync<ITicket>(new GetMixParlayDetailCriteria(transId, language));

            return tickets;
        }

        public async Task<IEnumerable<ITicket>> GetCombinationMixParlayDetail(long transId, string language)
        {
            var tickets = await dynamicRepository.FetchAsync<ITicket>(new GetCombinationMixParlayDetailCriteria(transId, language));

            return tickets;
        }
    }
}