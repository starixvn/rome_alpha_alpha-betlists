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
            ITicketHelper ticketHelper,
            string exportExcelURl = "Excel")
            : base(query.SportName, query.ViewerId, query.ViewerLevelId, param, detailData, builder, ticketHelper)
        {
            DisplayVNIP = query.DisplayVNIP;
            ExportExcelUrl = exportExcelURl;
            SportType = query.SportType;
        }

        public bool DisplayVNIP { get; }
        public string ExportExcelUrl { get; }
        public int SportType { get; }

        protected override string GetTitle()
        {
            if(SportType != 0)
            {
                return SportName;
            }

            return base.GetTitle();
        }

        public Components.PagerViewModel PagingViewModel =>
          new Components.PagerViewModel(PagerViewModel.PageIndex, PagerViewModel.PageSize, PagerViewModel.TotalRecords);
    }
}
