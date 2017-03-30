namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using AppResources;
    using BetLists.Models;
    using Configurations;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.WinLoss;
    using Web.Helpers;

    public class WinLossViewModel : BetListBaseViewModel
    {
        public WinLossViewModel(WinLossQuery query, BetListData betListData, IBetListHTMLBuilder builder, ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper)
        {
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public string Title => Resources.BetList;
    }
}