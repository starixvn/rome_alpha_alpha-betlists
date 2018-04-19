namespace Alpha.BetLists.Site.ViewModels.JackpotDetails
{
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.JackpotDetails;

    public class JackpotDetailsViewModel : BetListBaseViewModel
    {
        private readonly JackpotDetailsQuery query;

        public JackpotDetailsViewModel(
            JackpotDetailsQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false)
        {
            this.query = query;
            Title = Resources.ProgressiveJackpotWonDetails;
        }

        public long TransId => query.TransId;

        public int BetType => query.BetType;

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();
    }
}