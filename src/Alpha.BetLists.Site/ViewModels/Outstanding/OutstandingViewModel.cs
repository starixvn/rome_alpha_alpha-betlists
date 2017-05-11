namespace Alpha.BetLists.Site.ViewModels.Outstanding
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;

    public class OutstandingViewModel : BetListBaseViewModel
    {
        public OutstandingViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showMemberName = false)
            : base(query, betListData, builder, ticketHelper, showMemberName)
        {
        }
    }
}