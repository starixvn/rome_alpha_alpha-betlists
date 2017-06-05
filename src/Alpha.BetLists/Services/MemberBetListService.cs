namespace Alpha.BetLists.Services
{
    using Criteria.MemberBetList;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using System.Linq;
    using System.Threading.Tasks;

    public interface MemberBetListService
    {
        Task<BetListData> GetBetList(MemberBetListParam param);
    }

    public class MemberBetListServiceImpl : MemberBetListService
    {
        private readonly IDynamicRepository dynamicRepository;

        public MemberBetListServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(MemberBetListParam param)
        {
            var criteria = new GetBetListAllByDateCriteria(
                    param.CustId,
                    param.ProductIds.JoinWithComma(),
                    param.FromDate,
                    param.ToDate,
                    param.Language,
                    param.UserName);

            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}