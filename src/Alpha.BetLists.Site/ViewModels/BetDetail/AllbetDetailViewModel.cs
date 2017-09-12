namespace Alpha.BetLists.Site.ViewModels.BetDetail
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BetDetail;

    public class AllbetDetailViewModel : BetDetailViewModel
    {
        public AllbetDetailViewModel(
            AllbetDetailQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
        }
    }
}
