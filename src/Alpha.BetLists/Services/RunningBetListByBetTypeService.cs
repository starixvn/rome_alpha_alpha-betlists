namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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

        public RunningBetListByBetTypeServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetBetList(RunningBetListByBetTypeParam param)
            => await dynamicRepository.FetchAsync<TicketDto>(
                new GetBetListRunningByBetTypeCriteria(
                    param.CustId,
                    param.BetType,
                    param.CustLevel));
    }
}
