namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.WinLoss;

    public class WinLossHandler : BetListBaseHandler, IAsyncRequestHandler<WinLossQuery, WinLossViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;

        public WinLossHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            ProductService productService,
            ProductDetailService productDetailService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.winLossService = winLossService;
            this.productService = productService;
            this.productDetailService = productDetailService;
        }

        public async Task<WinLossViewModel> Handle(WinLossQuery message)
        {
            var productByHLParam = new GetProductsByGroupHeadlineParam(message.ViewerId, message.MinDate);
            var validUserSelectedProductIds = message.IsGroupHeadline
                ? productService.FilterProductIdsByGroupHeadline(message.ProductIds.ToListInt(), productByHLParam, message.IsEnableSabaPromotionForHeadline)
                : productService.FilterProductIdsByCustomer(message.ViewerId, message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt(), message.IsEnableSabaPromotion);
            validUserSelectedProductIds = await productDetailService.RemoveProductIdsAsync(validUserSelectedProductIds, message.ResourceIds);
            var productDetailsXML = await productDetailService.BuildProductDetailsXMLAsync(validUserSelectedProductIds, message.ResourceIds, message.Is789Y);

            var showContributionAmountRoundNote = message.IsGroupHeadline
                ? productService.IsEnabledJackpot(message.CustId)
                : message.IsEnabledJackpot;

            var winLossParam = CreateWinLossParam(message, validUserSelectedProductIds, productDetailsXML, message.MaxTotalRow, message.MaxTotalRowConfig);
            var betlistData = await winLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossViewModel(message, betlistData, Builder, TicketHelper, showContributionAmountRoundNote, message.IsGroupHeadline, betlistOutputParams: betlistData.BetlistOutputParams);
        }

        protected internal static WinLossParam CreateWinLossParam(WinLossQuery message, IEnumerable<int> productIds, string productDetailsXML, int maxTotalRow = 10000, int maxTotalRowConfig = 10000)
            => new WinLossParam(
               message.CustId,
               productIds,
               message.FromDate,               
               message.ToDate,
               message.LimitedDateToShowInfo,
               message.Language,
               productDetailsXML,
               maxTotalRow,
               maxTotalRowConfig);
    }
}