namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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

        public CorrectScoreServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(CorrectScoreParam param)
            => await dynamicRepository.FetchAsync<TicketDto>(
                new GetRunningCorrectScoreCriteria(
                    param.CustId,
                    param.Language,
                    param.CustLevel));
    }
}
