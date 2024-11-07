namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests.MonitoringBets;
    using Alpha.Web.Helpers;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class MonitoringBetsViewModel : FilteringBetDataViewModel
    {
        public MonitoringBetsViewModel(
            int filterId,
            IEnumerable<SavedFilter> filters,
            bool isShowFilterPlaceHolder,
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
            IsShowFilterPlaceHolder = isShowFilterPlaceHolder;
            MostRecentTickets = new[]
            {
                BuildSelectListItem(100, 100 == query.LimitRowCount),
                BuildSelectListItem(300, 300 == query.LimitRowCount),
                BuildSelectListItem(500, 500 == query.LimitRowCount)
            };
            AutoStart = query.AutoStart;
            IsInternal = query.IsInternal;
            ShutdownOldLastBetsKey = $"ShutdownOldLastBetsMessage-Hide-{query.ViewerId}-{query.SubAccId}";
        }

        public int FilterId { get; }

        public IEnumerable<SavedFilter> Filters { get; }

        public bool IsShowFilterPlaceHolder { get; }

        public IEnumerable<SelectListItem> MostRecentTickets { get; }

        public int[] AutoRefreshSeconds { get; } = new[] { 30, 60, 90 };

        public bool AutoStart { get; }

        public bool IsInternal { get; }

        private static SelectListItem BuildSelectListItem(int value, bool selected)
            => new SelectListItem { Text = value.ToString(), Value = value.ToString(), Selected = selected };

        public string UserGuideLink
            => AppSettings.IsMBCSite
            ? MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=186142")
            : MicroSites.MessageUrl("~/StaticMessage/Guide?id=186142");

        public string ShutdownOldLastBetsKey { get; }
    }
}