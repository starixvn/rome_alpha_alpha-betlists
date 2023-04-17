namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.BetLists.Models;
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

    public class WinLossHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<WinLossQuery, WinLossViewModel>,
        IAsyncRequestHandler<WinLossNewQuery, WinLossNewViewModel>
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
                ? productService.IsEnabledJackpot(message.Site, message.CurrencyId)
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

        public async Task<WinLossNewViewModel> Handle(WinLossNewQuery message)
        {
            var productsTask = productService.GetProductsForCustomerAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.MinDate,
                message.IsEnableSabaPromotion);
            var productDetailsTask = productDetailService.GetProductDetailsAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId);
            await Task.WhenAll(productsTask, productDetailsTask);
            var products = await productsTask;
            var productDetails = await productDetailsTask;

            var showContributionAmountRoundNote = message.IsGroupHeadline
                ? productService.IsEnabledJackpot(message.Site, message.CurrencyId)
                : message.IsEnabledJackpot;
            var winLossParam = await CreateWinLossParam(message, products, productDetails);
            var betlistData = await winLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossNewViewModel(
                message,
                BuildProductViewModels(message.ProductIds, products),
                BuildProductDetailViewModels(message.ResourceIds, await productDetailsTask),
                betlistData,
                Builder,
                TicketHelper,
                showContributionAmountRoundNote,
                message.IsGroupHeadline,
                betlistOutputParams: betlistData.BetlistOutputParams);
        }

        private async Task<WinLossParam> CreateWinLossParam(
            WinLossQuery message,
            IEnumerable<Product> products = null,
            IEnumerable<ProductDetail> productDetails = null)
        {
            var validUserSelectedProductIdsTask = productService.FilterProductIdsByCustomerAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.MinDate,
                message.ProductIds,
                message.IsEnableSabaPromotion,
                products);
            var validUserSelectedResourceIdsTask = productDetailService.FilterResourceIdsByCustomerAsync(
                message.ViewerId,
                message.Site,
                message.CurrencyId,
                message.ResourceIds,
                productDetails);
            await Task.WhenAll(validUserSelectedProductIdsTask, validUserSelectedResourceIdsTask);
            var validUserSelectedProductIds = await validUserSelectedProductIdsTask;
            var validUserSelectedResourceIds = await validUserSelectedResourceIdsTask;
            validUserSelectedProductIds = await productDetailService.RemoveProductIdsAsync(validUserSelectedProductIds, validUserSelectedResourceIds);

            return CreateWinLossParam(
                message,
                validUserSelectedProductIds,
                await productDetailService.BuildProductDetailsXMLAsync(validUserSelectedProductIds, message.ResourceIds, message.Is789Y),
                message.MaxTotalRow,
                message.MaxTotalRowConfig);
        }
    }
}