namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.WinLoss;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using Alpha.SmartPunter.Services;
    using Alpha.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;

    public class WinLossNewHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<WinLossNewQuery, WinLossNewViewModel>,
        IAsyncRequestHandler<WinLossGetDataQuery, WinLossViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;

        public WinLossNewHandler(
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

        public async Task<WinLossNewViewModel> Handle(WinLossNewQuery message)
        {
            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var showContributionAmountRoundNote = message.IsEnabledJackpot;
            var winLossParam = await CreateWinLossParam(message, products, productDetails);
            var betlistData = await winLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossNewViewModel(
                message,
                BuildProductViewModels(products, message),
                BuildProductDetailViewModels(productDetails, message.UserSelectedResourceIds),
                betlistData,
                Builder,
                TicketHelper,
                showContributionAmountRoundNote,
                message.IsGroupHeadline,
                betlistOutputParams: betlistData.BetlistOutputParams);
        }

        public async Task<WinLossViewModel> Handle(WinLossGetDataQuery message)
        {
            var showContributionAmountRoundNote = message.IsEnabledJackpot;
            var winLossParam = await CreateWinLossParam(message);
            var betlistData = await winLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossViewModel(
                message,
                betlistData,
                Builder,
                TicketHelper,
                showContributionAmountRoundNote,
                message.IsGroupHeadline,
                betlistOutputParams: betlistData.BetlistOutputParams);
        }

        private async Task<Tuple<IEnumerable<Product>, IEnumerable<ProductDetail>>> GetProductsAndProductDetails(
            WinLossGetDataQuery message)
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

            IEnumerable<Product> products = (await productsTask).ToList();
            IEnumerable<ProductDetail> productDetails = (await productDetailsTask).ToList();
            return Tuple.Create(products, productDetails);
        }

        private async Task<WinLossParam> CreateWinLossParam(WinLossGetDataQuery message)
        {
            var productsAndProductDetails = await GetProductsAndProductDetails(message);

            return await CreateWinLossParam(message, productsAndProductDetails.Item1, productsAndProductDetails.Item2);
        }

        private async Task<WinLossParam> CreateWinLossParam(
            WinLossGetDataQuery message,
            IEnumerable<Product> products,
            IEnumerable<ProductDetail> productDetails)
        {
            var validUserSelectedProductIds = GetValidUserSelectedProductIds(products, message.UserSelectedProductIds);
            var validUserSelectedResourceIds = GetValidUserSelectedResourceIds(productDetails, message.UserSelectedResourceIds);
            validUserSelectedProductIds = await productDetailService.RemoveProductIdsAsync(validUserSelectedProductIds, validUserSelectedResourceIds);

            return new WinLossParam(
                message.CustId,
                validUserSelectedProductIds,
                message.WeekFromDate ?? message.FromDate,
                message.WeekToDate ?? message.ToDate,
                message.LimitedDateToShowInfo,
                message.Language,
                await productDetailService.BuildProductDetailsXMLAsync(validUserSelectedProductIds, message.ResourceIds, message.Is789Y),
                message.MaxTotalRow,
                message.MaxTotalRowConfig);
        }

        private static IEnumerable<int> GetValidUserSelectedProductIds(
            IEnumerable<Product> products,
            IDictionary<int, bool> userSelectedProductIds)
            => products
            .Where(product => userSelectedProductIds.ContainsKey(product.Id))
            .Select(product => product.Id);

        private static IEnumerable<int> GetValidUserSelectedResourceIds(
            IEnumerable<ProductDetail> productDetails,
            IDictionary<int, bool> userSelectedResourceIds)
            => productDetails
            .Where(p => userSelectedResourceIds.ContainsKey(p.ResourceId))
            .Select(p => p.ResourceId);
    }
}