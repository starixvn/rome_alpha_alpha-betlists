namespace Alpha.BetLists.Site.ViewModels.BetDetail
{
    using BetLists.Models;
    using Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.BetDetail;

    public class BetDetailViewModel : BetListBaseViewModel
    {
        public BetDetailViewModel(
            BetDetailQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false)
        {
            PageIndex = query.PageIndex;
            PageSize = query.PageSize;
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public PagerViewModel PagerViewModel => new PagerViewModel(PageIndex, PageSize, TotalRows);
    }
}
