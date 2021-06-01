namespace Alpha.BetLists.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Web.Helpers;
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
        private readonly PartialUMService partialUMService;

        public RunningBetListServiceImpl(
            IDynamicRepository dynamicRepository,
            PartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<BetListData> GetBetList(RunningBetListParam param)
        {
            var criteria = new GetBetListRunningByDateCriteria(
                    param.CustId,
                    param.ProductIds.JoinWithComma(),
                    param.FromDate.GetGreaterDate(param.LimitedDate),
                    param.ToDate,
                    param.GetAll,
                    param.UserName);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}