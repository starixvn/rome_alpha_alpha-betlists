namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class MonitoringBetsViewModel : FilteringBetDataViewModel
    {
        public MonitoringBetsViewModel(
            int filterId,
            IEnumerable<SavedFilter> filters,
            long latestTransId,
            string lastTransIdCacheKeyByFilter,
            string latestTransIdCacheKeyByFilter,
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper) : base(
                latestTransId,
                lastTransIdCacheKeyByFilter,
                latestTransIdCacheKeyByFilter,
                query,
                betListData,
                builder,
                ticketHelper)
        {
            FilterId = filterId;
            Filters = filters;
            HasCustomFilter = filters?.Any(f => f.FromSession) == true;
        }

        public int FilterId { get; }

        public IEnumerable<SavedFilter> Filters { get; }

        public bool HasCustomFilter { get; }

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