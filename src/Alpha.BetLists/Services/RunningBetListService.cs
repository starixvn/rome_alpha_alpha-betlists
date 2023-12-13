namespace Alpha.BetLists.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.DataAccesses;
    using AlphaPlus.PartialUM.Services;
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
        private readonly IPartialUMService partialUMService;
        private readonly ParlayService parlayService;
        private readonly IBetListRepository betlistRepository;

        public RunningBetListServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            ParlayService parlayService,
            IBetListRepository betlistRepository)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.parlayService = parlayService;
            this.betlistRepository = betlistRepository;
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

            partialUMService.ThrowIfUM(criteria.GetSettingKey());

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            //var betListDataTuple = betlistRepository.FetchMultiple(criteria);
            await parlayService.UpdateOddsMixParlay(betListDataTuple.Item1);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}