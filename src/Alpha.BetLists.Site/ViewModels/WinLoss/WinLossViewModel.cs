namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.WinLoss;

    public class WinLossViewModel : BetListBaseViewModel
    {
        public WinLossViewModel(WinLossQuery query, BetListData betListData, IBetListHTMLBuilder builder, ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
            Title = GetTitle(query).ToUpperInvariant();
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public string Title { get; }

        private string GetTitle(WinLossQuery query)
            => $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}";
    }
}