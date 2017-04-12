namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;

    public interface MixParlayService
    {
        Task<IEnumerable<ITicket>> GetParlayDetail(long transId, string language);
    }

    public class MixParlayServiceImpl : MixParlayService
    {
        private readonly IDynamicRepository dynamicRepository;

        public MixParlayServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<ITicket>> GetParlayDetail(long transId, string language)
        {
            var tickets = await dynamicRepository.FetchAsync<ITicket>(new GetMixParlayDetailCriteria(transId, language));

            return tickets;
        }
    }
}