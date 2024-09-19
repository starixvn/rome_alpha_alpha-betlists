namespace Alpha.BetLists.Site.ViewModels.CombinationMixParlay
{
    using System.Collections.Generic;
    using AppResources;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.CombinationMixParlay;

    public class CombinationMixParlayViewModel : BetListByBetTypeViewModel
    {
        public CombinationMixParlayViewModel(
            CombinationMixParlayQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBetTypeIds, supportRiskAmount: true)
        {
            Title = Resources.parlay.ToUpperInvariant();
        }
    }
}
