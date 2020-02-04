namespace Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;
    using System;

    public class RiskAnalysisIPMatchingViewModel : WinLossViewModel
    {
        public RiskAnalysisIPMatchingViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showContributionAmountRoundNote,
            DateTime date,
            string ip)
            : base(query, betListData, builder, ticketHelper, showContributionAmountRoundNote, true)
        {
            Date = date;
            IP = ip;
            Title = "RiskAnalysisIPMatching";
        }

        public DateTime Date { get; }

        public string IP { get; }
    }
}