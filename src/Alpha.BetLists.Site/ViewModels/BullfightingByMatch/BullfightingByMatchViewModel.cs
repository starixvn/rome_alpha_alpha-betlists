namespace Alpha.BetLists.Site.ViewModels.BullfightingByMatch
{
    using System.Collections.Generic;
    using AppResources;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BullfightingByMatch;

    public class BullfightingByMatchViewModel : BetListByBetTypeViewModel
    {
        public BullfightingByMatchViewModel(
            BullfightingByMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBetTypeIds)
        {
            MatchId = query.MatchId;
            Title = Resources.Bullfighting.ToUpperInvariant();
        }

        public int MatchId { get; }
    }
}
