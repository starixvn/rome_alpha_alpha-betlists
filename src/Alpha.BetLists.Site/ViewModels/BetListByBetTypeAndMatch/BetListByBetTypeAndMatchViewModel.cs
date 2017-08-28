namespace Alpha.BetLists.Site.ViewModels.BetListByBetTypeAndMatch
{
    using System.Collections.Generic;
    using BetListByBetType;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BetListByBetTypeAndMatch;

    public class BetListByBetTypeAndMatchViewModel : BetListByBetTypeViewModel
    {
        public BetListByBetTypeAndMatchViewModel(
            BetListByBetTypeAndMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBettypeIds = null)
            : base(query, betListData, builder, ticketHelper, supportScoreMapBettypeIds)
        {
            MatchId = query.MatchId;
            IsLive = query.IsLive;
            BetTeam = query.BetTeam;
        }

        public int MatchId { get; }

        public int IsLive { get; }

        public string BetTeam { get; }
    }
}
