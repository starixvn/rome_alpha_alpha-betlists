namespace Alpha.BetLists.Services
{
    using Criteria.Outstanding;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using System.Linq;
    using System.Threading.Tasks;

    public interface OutstandingService
    {
        Task<BetListData> GetBetList(OutstandingParam param);
    }

    public class OutstandingServiceImpl : OutstandingService
    {
        private readonly IDynamicRepository dynamicRepository;

        public OutstandingServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetList(OutstandingParam param)
        {
            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<Ticket, TicketData>(
                new GetBetListRunningByDateCriteria(
                    param.CustId,
                    param.ProductIds.JoinWithComma(),
                    param.FromDate,
                    param.ToDate,
                    param.GetAll,
                    param.Language,
                    param.UserName));

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}