namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class MonitoringBetsViewModel : BetListBaseViewModel
    {
        public MonitoringBetsViewModel(long latestTransId,
            MonitoringBetsQuery query,
            IEnumerable<SavedFilter> filters,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            string lastTransIdCacheKeyByFilter,
            string latestTransIdCacheKeyByFilter,
            bool showMemberName = false) : base(query, betListData, builder, ticketHelper, showMemberName)
        {
            LatestTransId = latestTransId;
            Filters = filters;
            TotalColumn = 8;
            LastTransIdCacheKeyByFilter = lastTransIdCacheKeyByFilter;
            LatestTransIdCacheKeyByFilter = latestTransIdCacheKeyByFilter;
        }

        public IEnumerable<SavedFilter> Filters { get; }

        public string LastTransIdCacheKeyByFilter { get; }

        public string LatestTransIdCacheKeyByFilter { get; }
        
        public long LatestTransId { get; }
        public long LastTransId
            => BetListData.Tickets.Any()
            ? BetListData.Tickets.ElementAt(BetListData.Total - 1).TransId
            : 0;

        public int[] AutoRefreshSeconds { get; } = new[] { 30, 60, 90 };

        public IEnumerable<SelectListItem> MostRecentTickets { get; } = new[]
        {
            BuildSelectListItem(100, true),
            BuildSelectListItem(300),
            BuildSelectListItem(500)
        };

        private static SelectListItem BuildSelectListItem(int value, bool selected = false)
            => new SelectListItem { Text = value.ToString(), Value = value.ToString(), Selected = selected };
    }
}