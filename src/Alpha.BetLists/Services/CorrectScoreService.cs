namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
    using AlphaPlus.PartialUM.Services;
    using Criteria.CorrectScore;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface CorrectScoreService
    {
        Task<IEnumerable<TicketDto>> GetBetList(CorrectScoreParam param);
    }

    public class CorrectScoreServiceImpl : CorrectScoreService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private readonly IBetListRepository betlistRepository;

        public CorrectScoreServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.betlistRepository = betlistRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(CorrectScoreParam param)
        {
            var criteria = new GetRunningCorrectScoreCriteria(param.CustId, param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchAsync<TicketDto>(criteria);
            //return betlistRepository.Fetch(criteria);
        }
    }
}
