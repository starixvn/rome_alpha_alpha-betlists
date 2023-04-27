namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using System.Collections.Generic;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests.WinLoss;
    using Alpha.BetLists.Site.ViewModels.Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class WinLossNewViewModel : WinLossViewModel
    {
        public WinLossNewViewModel(
            WinLossNewQuery query,
            IEnumerable<ProductViewModel> products,
            IEnumerable<ProductDetailViewModel> productDetails,
            BetListData betListData,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            bool showContributionAmountRoundNote,
            bool isGroupHeadline = false,
            bool showMemberName = false,
            string[] excludingTicketStatus = null,
            BetlistOutputParams betlistOutputParams = null)
            : base(
                query,
                betListData,
                builder,
                ticketHelper,
                showContributionAmountRoundNote,
                isGroupHeadline,
                showMemberName,
                excludingTicketStatus,
                betlistOutputParams)
        {
            BreadcrumbLevelJson = query.BreadcrumbLevelJson;
            DateRangePickerViewModel = new DateRangePickerNewViewModel(
                query.FromDate,
                query.ToDate,
                query.HistoricPivotDate,
                query.MinDate);
            Products = products;
            ProductDetails = productDetails;
            WeekFromDate = query.WeekFromDate.HasValue ? query.WeekFromDate.Value.ToShortDateWithSlash() : string.Empty;
            WeekToDate = query.WeekToDate.HasValue ? query.WeekToDate.Value.ToShortDateWithSlash() : string.Empty;
        }

        public string BreadcrumbLevelJson { get; set; }

        public DateRangePickerNewViewModel DateRangePickerViewModel { get; }

        public IEnumerable<ProductViewModel> Products { get; }

        public IEnumerable<ProductDetailViewModel> ProductDetails { get; }

        public string WeekFromDate { get; }

        public string WeekToDate { get; }
    }
}