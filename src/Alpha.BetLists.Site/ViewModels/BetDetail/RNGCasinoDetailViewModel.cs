namespace Alpha.BetLists.Site.ViewModels.BetDetail
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BetDetail;

    public class RNGCasinoDetailViewModel : BetDetailViewModel
    {
        public RNGCasinoDetailViewModel(
            RNGCasinoDetailQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
            Title = (!string.IsNullOrEmpty(query.SportName) ? query.SportName : "RNG Casino") + " Details";
        }
    }
}
