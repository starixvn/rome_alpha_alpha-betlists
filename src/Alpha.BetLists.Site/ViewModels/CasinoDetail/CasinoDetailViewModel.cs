namespace Alpha.BetLists.Site.ViewModels.CasinoDetail
{
    using BetLists.Models;
    using Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.CasioDetail;

    public class CasinoDetailViewModel : BetListBaseViewModel
    {
        public CasinoDetailViewModel(
            CasioDetailQuery query,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query, betListData, builder, ticketHelper, false)
        {
            PageIndex = query.PageIndex;
            PageSize = query.PageSize;
            Title = (!string.IsNullOrEmpty(query.SportName) ? query.SportName : "RNG Casino") + " Details";
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        public bool AllowToViewSuperPT => baseQuery.ViewerLevel.IsSuperLevel();

        public bool AllowToViewMasterPT => baseQuery.ViewerLevel.IsMasterOrSuperLevel();

        public PagerViewModel PagerViewModel => new PagerViewModel(PageIndex, PageSize, TotalRows);
    }
}
