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
            bool showContributionAmountRoundNote,
            bool isGroupHeadline = false,
            bool showMemberName = false,
            string[] excludingTicketStatus = null,
            BetlistOutputParams betlistOutputParams = null,
            bool showFooter = false,
            bool showOrder = false,
            bool supportRiskAmount = false)
            : base(query, betListData, builder, ticketHelper, showMemberName, supportRiskAmount)
        {
            Footer = new Footer(betListData, baseQuery.ViewerLevel, showMemberName, excludingTicketStatus, supportRiskAmount);
            ShowContributionAmountRoundNote = showContributionAmountRoundNote;
            IsGroupHeadline = isGroupHeadline;
            BetlistOutputParams = betlistOutputParams ?? new BetlistOutputParams();
            ShowFooter = showFooter;
            ShowOrder = showOrder;
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public bool ShowContributionAmountRoundNote { get; }

        public bool IsGroupHeadline { get; }

        public Footer Footer { get; }

        public BetlistOutputParams BetlistOutputParams { get; }
        public bool ShowFooter { get; }
        public bool ShowOrder { get; }
    }
}