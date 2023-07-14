namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests;
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

            LastTransIdCacheKeyByFilter = lastTransIdCacheKeyByFilter;
            LatestTransIdCacheKeyByFilter = latestTransIdCacheKeyByFilter;
        }

        public IEnumerable<SavedFilter> Filters { get; }

        public string LastTransIdCacheKeyByFilter { get; }

        public string LatestTransIdCacheKeyByFilter { get; }
    }
}