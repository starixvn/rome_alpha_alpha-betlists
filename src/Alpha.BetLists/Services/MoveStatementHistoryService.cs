namespace Alpha.BetLists.Services
{
    using Alpha.BetLists.Criteria.MoveStatementHistory;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.Web.Helpers;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface MoveStatementHistoryService
    {
        Task<IEnumerable<TicketDto>> GetBetList(MoveStatementHistoryParam param);
    }

    public class MoveStatementHistoryServiceImpl : MoveStatementHistoryService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly PartialUMService partialUMService;

        public MoveStatementHistoryServiceImpl(IDynamicRepository dynamicRepository, PartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(MoveStatementHistoryParam param)
        {
            var criteria = new GetBetListMoveStatementHistoryCriteria(param.CustId, param.TransId);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchAsync<TicketDto>(criteria);
        }
    }
}
