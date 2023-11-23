namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.PartialUM.Services;
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
        private readonly IPartialUMService partialUMService;
        private readonly ParlayService parlayService;

        public RunningBetListByBetTypeServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            ParlayService parlayService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.parlayService = parlayService;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(RunningBetListByBetTypeParam param)
        {
            var criteria = new GetBetListRunningByBetTypeCriteria(param.CustId, param.BetType, param.CustLevel);

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);

            await parlayService.UpdateOddsMixParlay(tickets);

            return tickets;
        }
    }
}
