namespace Alpha.BetLists.Site.ViewModels.RacingByMatch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.RacingByMatch;
    using WinLoss;

    public class RacingByMatchViewModel : WinLossViewModel
    {
        private static readonly string[] ExcludingTicketStatus =
        {
            BetStatus.Running,
            BetStatus.Waiting,
            BetStatus.Void,
            BetStatus.Reject
        };

        public RacingByMatchViewModel(
            RacingByMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(query, betListData, builder, ticketHelper, true, ExcludingTicketStatus)
        {
            BetType = query.BetType;
            MatchId = query.MatchId;
            Title = Resources.horseRacing.ToUpperInvariant();
            SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;
        }

        public int BetType { get; }

        public int MatchId { get; }

        public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

        public override bool CanViewScoreMap(ITicket ticket)
            => SupportScoreMapBetTypeIds != null
            && SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId)
            && ticket.Status.Equals(BetStatus.Running, StringComparison.OrdinalIgnoreCase);
    }
}
