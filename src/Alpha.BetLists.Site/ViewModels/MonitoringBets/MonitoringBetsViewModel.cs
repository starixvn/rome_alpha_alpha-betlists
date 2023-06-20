namespace Alpha.BetLists.Site.ViewModels.MonitoringBets
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class MonitoringBetsViewModel : BetListBaseViewModel
    {
        public MonitoringBetsViewModel(
            BaseQuery message,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showMemberName = false) : base(message, betListData, builder, ticketHelper, showMemberName)
        {
        }
    }
}