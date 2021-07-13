namespace Alpha.BetLists.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Web.Helpers;
    using Criteria.MemberBetList;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface MemberBetListService
    {
        Task<BetListData> GetBetList(MemberBetListParam param);
    }

    public class MemberBetListServiceImpl : MemberBetListService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly PartialUMService partialUMService;

        public MemberBetListServiceImpl(
            IDynamicRepository dynamicRepository,
            PartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<BetListData> GetBetList(MemberBetListParam param)
        {
            var criteria = new GetBetListAllByDateCriteria(
                    param.CustId,
                    param.ProductIds.JoinWithComma(),
                    param.FromDate.GetGreaterDate(param.LimitedDate),
                    param.ToDate,
                    param.UserName);

            partialUMService.CheckPartialUM(criteria.GetSettingKey());

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}