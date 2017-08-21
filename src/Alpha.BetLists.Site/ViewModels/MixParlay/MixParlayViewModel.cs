namespace Alpha.BetLists.Site.ViewModels.MixParlay
{
    using System.Collections.Generic;
    using AppResources;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.MixParlay;

    public class MixParlayViewModel : BetListByBetTypeViewModel
    {
        public MixParlayViewModel(
            MixParlayQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBettypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBettypeIds)
        {
            Title = Resources.lblMixParlay.ToUpperInvariant();
        }
    }
}
