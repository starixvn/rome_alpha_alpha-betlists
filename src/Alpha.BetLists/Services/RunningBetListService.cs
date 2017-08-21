namespace Alpha.BetLists.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.RunningBetList;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface RunningBetListService
    {
        Task<BetListData> GetBetList(RunningBetListParam param);
    }

    public class RunningBetListServiceImpl : RunningBetListService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RunningBetListServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(RunningBetListParam param)
        {
            var criteria = new GetBetListRunningByDateCriteria(
                    param.CustId,
                    param.ProductIds.JoinWithComma(),
                    param.FromDate,
                    param.ToDate,
                    param.GetAll,
                    param.Language,
                    param.UserName);

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}