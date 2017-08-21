namespace Alpha.BetLists.Services
{
    using System.Threading.Tasks;
    using Criteria.RunningBetListByBetType;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface RunningBetListByBetTypeService
    {
        Task<BetListData> GetBetList(RunningBetListByBetTypeParam param);
    }

    public class RunningBetListByBetTypeServiceImpl : RunningBetListByBetTypeService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RunningBetListByBetTypeServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(RunningBetListByBetTypeParam param)
        {
            var tickets = await dynamicRepository.FetchAsync<TicketDto>(
                new GetBetListRunningByBetTypeCriteria(
                    param.CustId,
                    param.BetType,
                    param.Language,
                    param.CustLevel));

            return new BetListData(tickets, null);
        }
    }
}
