namespace Alpha.BetLists.Site.ViewModels.MoveStatementHistory
{
    using Alpha.BetLists.AppResources;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class MoveStatementHistoryViewModel : BetListBaseViewModel
    {
        public MoveStatementHistoryViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper
            ) : base(query, betListData, builder, ticketHelper)
        {
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public string GetMoveStatementHistoryTitle => CustName.ToUpperInvariant() + " - " + Resources.TicketDetails.ToUpperInvariant();
    }
}