namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Web.Helpers;
    using Criteria.RunningBetListByBetType;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface RunningBetListByBetTypeService
    {
        Task<IEnumerable<TicketDto>> GetBetList(RunningBetListByBetTypeParam param);
    }

    public class RunningBetListByBetTypeServiceImpl : RunningBetListByBetTypeService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly PartialUMService partialUMService;

        public RunningBetListByBetTypeServiceImpl(
            IDynamicRepository dynamicRepository,
            PartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(RunningBetListByBetTypeParam param)
        {
            var criteria = new GetBetListRunningByBetTypeCriteria(param.CustId, param.BetType, param.CustLevel);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            return await dynamicRepository.FetchAsync<TicketDto>(criteria);
        }
    }
}
