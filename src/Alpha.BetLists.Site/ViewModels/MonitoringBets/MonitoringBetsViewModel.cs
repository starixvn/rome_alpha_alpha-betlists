namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
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
            MonitoringBetsQuery query,
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
            MostRecentTickets = new[]
            {
                BuildSelectListItem(100, 100 == query.LimitRowCount),
                BuildSelectListItem(300, 300 == query.LimitRowCount),
                BuildSelectListItem(500, 500 == query.LimitRowCount)
            };
            AutoStart = query.AutoStart;
        }

        public int FilterId { get; }

        public IEnumerable<SavedFilter> Filters { get; }

        public IEnumerable<SelectListItem> MostRecentTickets { get; }

        public int[] AutoRefreshSeconds { get; } = new[] { 30, 60, 90 };

        public bool AutoStart { get; }

        private static SelectListItem BuildSelectListItem(int value, bool selected)
            => new SelectListItem { Text = value.ToString(), Value = value.ToString(), Selected = selected };
    }
}