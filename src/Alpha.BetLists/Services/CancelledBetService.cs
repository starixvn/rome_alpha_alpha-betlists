namespace Alpha.BetLists.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.CancelledBet;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;

    public interface CancelledBetService
    {
        Task<PagedReportData<BetListData>> GetCancelledBet(CancelledBetParam parameter);
    }

    public class CancelledBetServiceImpl : CancelledBetService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CancelledBetServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<PagedReportData<BetListData>> GetCancelledBet(CancelledBetParam parameter)
        {
            var criteria = new GetCancelledBetCriteria(
                parameter.CustId,
                parameter.FromDate,
                parameter.ToDate,
                parameter.MoveBetsDay,
                parameter.RoleId,
                parameter.ProductIds.JoinWithComma(),
                parameter.StatusIds,
                parameter.PageSize,
                parameter.PageIndex);
            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
            return new PagedReportData<BetListData>(
                new List<BetListData>
                {
                    new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>())
                },
                criteria.PageIndex,
                criteria.PageSize,
                criteria.TotalRows);
        }
    }
}
