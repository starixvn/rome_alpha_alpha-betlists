namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using BetLists.Models;
    using Blocks;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;

    public class WinLossViewModel : BetListBaseViewModel
    {
        public WinLossViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showMemberName = false,
            string[] excludingTicketStatus = null)
            : base(query, betListData, builder, ticketHelper, showMemberName)
        {
            Footer = new Footer(betListData, baseQuery.ViewerLevel, showMemberName, excludingTicketStatus);
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public Footer Footer { get; }
    }
}