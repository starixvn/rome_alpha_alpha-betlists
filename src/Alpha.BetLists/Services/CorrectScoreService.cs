namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.PartialUM.Services;
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

        public CorrectScoreServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(CorrectScoreParam param)
        {
            var criteria = new GetRunningCorrectScoreCriteria(param.CustId, param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchAsync<TicketDto>(criteria);
        }
    }
}
