namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class MonitoringBetsViewModel : BetListBaseViewModel
    {
        public MonitoringBetsViewModel(
            MonitoringBetsQuery query,
            IEnumerable<SavedFilter> filters,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            string lastTransIdCacheKeyByFilter,
            string latestTransIdCacheKeyByFilter,
            bool showMemberName = false) : base(query, betListData, builder, ticketHelper, showMemberName)
        {
            Filters = filters;
            TotalColumn = 8;
            LastTransIdCacheKeyByFilter = lastTransIdCacheKeyByFilter;
            LatestTransIdCacheKeyByFilter = latestTransIdCacheKeyByFilter;
        }

        public IEnumerable<SavedFilter> Filters { get; }

        public string LastTransIdCacheKeyByFilter { get; }

        public string LatestTransIdCacheKeyByFilter { get; }

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