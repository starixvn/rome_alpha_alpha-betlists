namespace Alpha.BetLists.Site.ViewModels.WinLoss
{
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests.WinLoss;
    using Alpha.BetLists.Site.ViewModels.Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class WinLossIndexViewModel : WinLossBetListWeekSelectionViewModel
    {
        public WinLossIndexViewModel(
            WinLossIndexQuery query,
            IEnumerable<ProductViewModel> products,
            IEnumerable<ProductDetailViewModel> productDetails,
            IEnumerable<WeekSelectionViewModel> weekSelections,
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
                weekSelections,
                betListData,
                builder,
                ticketHelper,
                showContributionAmountRoundNote,
                isGroupHeadline,
                showMemberName,
                excludingTicketStatus,
                betlistOutputParams)
        {
            DateRangePickerViewModel = new DateRangePickerNewViewModel(
                query.FromDate,
                query.ToDate,
                query.MinDate);
            Products = products;
            ProductDetails = productDetails;
            Language = query.Language;
            ShowColumns = query.ShowColumns;
        }

        public DateRangePickerNewViewModel DateRangePickerViewModel { get; }

        public IEnumerable<ProductViewModel> Products { get; }

        public IEnumerable<ProductDetailViewModel> ProductDetails { get; }

        public string Language { get; }

        public string ShowColumns { get; }
    }
}