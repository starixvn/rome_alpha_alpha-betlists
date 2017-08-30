namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria.RacingByMatch;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface RacingByMatchService
    {
        Task<IEnumerable<TicketDto>> GetBetList(RacingByMatchParam param);
    }

    public class RacingByMatchServiceImpl : RacingByMatchService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RacingByMatchServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(RacingByMatchParam param)
            => await dynamicRepository.FetchAsync<TicketDto>(
                new GetBetListRacingByMatchCriteria(
                    param.CustId,
                    param.BetType,
                    param.MatchId,
                    param.Language,
                    param.IsShowRacing,
                    param.CustLevel));
    }
}
