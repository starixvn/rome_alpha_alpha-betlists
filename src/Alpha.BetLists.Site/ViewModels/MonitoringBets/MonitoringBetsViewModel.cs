namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Site.Requests;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class MonitoringBetsViewModel : BetListBaseViewModel
    {
        public MonitoringBetsViewModel(
            BaseQuery message,
            IEnumerable<FilterConfiguration> filters,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showMemberName = false) : base(message, betListData, builder, ticketHelper, showMemberName)
        {
            Filters = filters;
        }

        public IEnumerable<FilterConfiguration> Filters { get; }
    }
}