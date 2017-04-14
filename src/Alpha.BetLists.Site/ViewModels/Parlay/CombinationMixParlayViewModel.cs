namespace Alpha.BetLists.Site.ViewModels.Parlay
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;

    public class CombinationMixParlayViewModel : BetListBaseViewModel
    {
        public CombinationMixParlayViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
        }

        public string OfferNames { get; set; }
    }
}