namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.LastBets;

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