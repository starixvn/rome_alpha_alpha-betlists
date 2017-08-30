namespace Alpha.BetLists.Site.ViewModels.RacingByMatch
{
    using System.Collections.Generic;
    using AppResources;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BetListByBetType;

    public class RacingByMatchViewModel : BetListByBetTypeViewModel
    {
        public RacingByMatchViewModel(
            BetListByBetTypeQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBettypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBettypeIds)
        {
            Title = Resources.horseRacing;
        }

        public int MatchId { get; }
    }
}
