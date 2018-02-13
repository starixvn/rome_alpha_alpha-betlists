namespace Alpha.BetLists.Site.ViewModels.JackpotDetails
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests;

    public class JackpotDetailsViewModel : BetListBaseViewModel
    {
        public JackpotDetailsViewModel(
            BaseQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false)
        {
        }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();
    }
}