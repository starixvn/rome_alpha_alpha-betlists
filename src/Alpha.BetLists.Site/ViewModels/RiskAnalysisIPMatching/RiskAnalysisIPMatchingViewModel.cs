namespace Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using System;

    public class RiskAnalysisIPMatchingViewModel : WinLossViewModel
    {
        private static readonly string[] excludingTicketStatus = { "running", "waiting" };

        public RiskAnalysisIPMatchingViewModel(
            RiskAnalysisIPMatchingQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false, true, excludingTicketStatus)
        {
            Date = query.Date;
            IP = query.IP;
            Title = String.Format(AppResources.Resources.IPMatchingTitle, query.Date.ToShortDateString(), IP);
        }

        public DateTime Date { get; }

        public string IP { get; }
    }
}