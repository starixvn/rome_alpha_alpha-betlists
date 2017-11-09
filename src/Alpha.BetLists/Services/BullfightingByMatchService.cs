namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.RacingByMatch;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface BullfightingByMatchService
    {
        Task<IEnumerable<TicketDto>> GetBetList(BullfightingByMatchParam param);
    }

    public class BullfightingByMatchServiceImpl : BullfightingByMatchService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BullfightingByMatchServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(BullfightingByMatchParam param)
            => await dynamicRepository.FetchAsync<TicketDto>(
                new GetBetListBullfightingByMatchCriteria(
                    param.CustId,                    
                    param.MatchId,
                    param.CustLevel));
    }
}
