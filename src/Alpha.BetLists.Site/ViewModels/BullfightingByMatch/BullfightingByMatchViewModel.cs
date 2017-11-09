namespace Alpha.BetLists.Site.ViewModels.BullfightingByMatch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BullfightingByMatch;
    using WinLoss;

    public class BullfightingByMatchViewModel : WinLossViewModel
    {
        private static readonly string[] ExcludingTicketStatus =
        {
            BetStatus.Running,
            BetStatus.Waiting,
            BetStatus.Void,
            BetStatus.Reject
        };

        public BullfightingByMatchViewModel(
            BullfightingByMatchQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, true, ExcludingTicketStatus)
        {
            MatchId = query.MatchId;
            Title = Resources.Bullfighting.ToUpperInvariant();
        }        

        public int MatchId { get; }        
    }
}
