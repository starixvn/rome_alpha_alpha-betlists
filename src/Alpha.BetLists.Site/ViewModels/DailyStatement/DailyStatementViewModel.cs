namespace Alpha.BetLists.Site.ViewModels.DailyStatement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Blocks;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.DailyStatement;

    public class DailyStatementViewModel : BetListBaseViewModel
    {
        private static readonly string[] ExcludedTicketStatus =
        {
            BetStatus.Running,
            BetStatus.Waiting,
            BetStatus.Void,
            BetStatus.Reject
        };

        private readonly DailyStatementQuery query;

        public DailyStatementViewModel(
            DailyStatementQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IEnumerable<int> supportScoreMapBetTypeIds = null)
            : base(query, betListData, builder, ticketHelper, false)
        {
            this.query = query;

            SupportScoreMapBetTypeIds = supportScoreMapBetTypeIds;

            Footer = new Footer(betListData, baseQuery.ViewerLevel, false, ExcludedTicketStatus);
            TransType = query.TransType;
            Title = $"{Resources.DailyStatement} {query.WinLossDate.ToShortDateString()} - {CustName}";
        }

        public DateTime WinLossDate => query.WinLossDate;

        public int TransType { get; }

        public IEnumerable<int> SupportScoreMapBetTypeIds { get; }

        public Footer Footer { get; }

        public bool ShowContributionAmountRoundNote => query.IsEnabledJackpot;

        public override bool CanViewScoreMap(ITicket ticket)
            => string.Equals(ticket.Status, BetStatus.Running, StringComparison.OrdinalIgnoreCase)
            && SupportScoreMapBetTypeIds != null
            && SupportScoreMapBetTypeIds.Contains(ticket.BetTypeId);
    }
}