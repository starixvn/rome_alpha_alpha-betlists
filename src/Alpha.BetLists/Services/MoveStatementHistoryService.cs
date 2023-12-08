namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.MoveStatementHistory;
    using Alpha.BetLists.DataAccesses;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;

    public interface MoveStatementHistoryService
    {
        Task<IEnumerable<TicketDto>> GetBetList(MoveStatementHistoryParam param);
    }

    public class MoveStatementHistoryServiceImpl : MoveStatementHistoryService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;
        private readonly IBetListRepository betlistRepository;

        public MoveStatementHistoryServiceImpl(
            IDynamicRepository dynamicRepository, 
            IPartialUMService partialUMService,
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.betlistRepository = betlistRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(MoveStatementHistoryParam param)
        {
            var criteria = new GetBetListMoveStatementHistoryCriteria(param.CustId, param.TransId);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            //return await dynamicRepository.FetchAsync<TicketDto>(criteria);
            return betlistRepository.Fetch(criteria);
        }
    }
}
