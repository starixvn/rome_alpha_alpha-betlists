namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Linq;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class FilteringBetDataViewModel : BetListBaseViewModel
    {
        public FilteringBetDataViewModel(
            long latestTransId,
            string lastTransIdCacheKeyByFilter,
            string latestTransIdCacheKeyByFilter,
            GetLastFilteringBetsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, showMemberName: true)
        {
            LatestTransId = latestTransId;
            LastTransIdCacheKeyByFilter = lastTransIdCacheKeyByFilter;
            LatestTransIdCacheKeyByFilter = latestTransIdCacheKeyByFilter;

            TotalColumn = 8;
        }

        public long LatestTransId { get; }

        public string LastTransIdCacheKeyByFilter { get; }

        public string LatestTransIdCacheKeyByFilter { get; }

        public long LastTransId
            => BetListData.Tickets.Any()
            ? BetListData.Tickets.ElementAt(BetListData.Total - 1).TransId
            : 0;
    }
}