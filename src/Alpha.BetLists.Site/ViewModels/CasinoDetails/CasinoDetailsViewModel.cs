namespace Alpha.BetLists.Site.ViewModels.CasinoDetails
{
    using Fanex.BetDetails.Casino.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.ViewModels;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Requests.CasinoDetails;

    public class CasinoDetailsViewModel : CasinoDetailsBaseViewModel
    {
        public CasinoDetailsViewModel(
            CasinoDetailsQuery query,
            CasinoDetailsParam param,
            PagedData<ITicket> detailData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
            : base(query.SportName, query.ViewerId, query.ViewerLevelId, param, detailData, builder, ticketHelper)
        {
            DisplayVNIP = query.DisplayVNIP;
        }

        public bool DisplayVNIP { get; }

        public PagerViewModel PagingViewModel =>
           new PagerViewModel(PagerViewModel.PageIndex, PagerViewModel.PageSize, PagerViewModel.TotalRecords);
    }
}
