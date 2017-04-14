namespace Alpha.BetLists.Site.ViewModels.Parlay
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;

    public class MixParlayViewModel : BetListBaseViewModel
    {
        public MixParlayViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
        }
    }
}