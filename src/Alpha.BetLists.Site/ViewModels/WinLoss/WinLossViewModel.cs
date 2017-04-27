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
            bool showMemberName = false)
            : base(query, betListData, builder, ticketHelper)
        {
            Footer = new Footer(betListData, baseQuery.ViewerLevel);

            var numberOfMemberColumns = showMemberName ? 1 : 0;


            FooterSubTotalColSpan = 5 + numberOfMemberColumns;
            ShowMemberName = showMemberName;
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public Footer Footer { get; }
        public bool ShowMemberName { get; }
    }
}