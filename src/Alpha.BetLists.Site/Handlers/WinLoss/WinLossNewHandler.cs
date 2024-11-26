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
    using Alpha.BetLists.Site.ViewModels.Components;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;

    public class WinLossNewHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<WinLossIndexQuery, WinLossIndexViewModel>,
        IAsyncRequestHandler<WinLossBetListWeekSelectionQuery, WinLossBetListWeekSelectionViewModel>,
        IAsyncRequestHandler<WinLossWeeksQuery, IEnumerable<WeekSelectionViewModel>>,
        IAsyncRequestHandler<WinLossBetListQuery, WinLossViewModel>
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
            ProductDetailService productDetailService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.winLossService = winLossService;
            this.productService = productService;
            this.productDetailService = productDetailService;
        }

        public async Task<WinLossIndexViewModel> Handle(WinLossIndexQuery message)
        {
            var showContributionAmountRoundNote = message.IsEnabledJackpot;
            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var validUserSelectedProductIds = GetValidUserSelectedProductIds(products, message.UserSelectedProductIds);
            var validResourceIds = await productDetailService.FilterResourceIdsByCustomer(message.Site, message.CurrencyId, message.ResourceIds);
            var productDetailsXML = await productDetailService.BuildProductDetailsXMLAsync(validResourceIds, message.Is789Y);

            var winLossWeekDataParam = new WinLossWeekDataParam(
                message.ViewerId,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                productDetailsXML,
                message.CustName);
            var weekData = await winLossService.GetWinLossWeekData(winLossWeekDataParam);
            BetListData betlistData = new BetListData(Enumerable.Empty<TicketDto>(), new List<ITicketData>());

            if (weekData.Any())
            {
                var winLossParam = new WinLossParam(
                    message.CustId,
                    validUserSelectedProductIds,
                    message.WeekFromDate ?? message.FromDate,
                    message.WeekToDate ?? message.ToDate,
                    message.LimitedDateToShowInfo,
                    message.Language,
                    productDetailsXML,
                    message.Site,
                    message.MaxTotalRow,
                    message.MaxTotalRowConfig);

                betlistData = await winLossService.GetBetList(winLossParam);
                await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

                WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);
            }

            return new WinLossIndexViewModel(
                message,
                BuildProductViewModels(products, message),
                BuildProductDetailViewModels(productDetails, message.UserSelectedResourceIds),
                BuildWeekSelectionViewModels(weekData, message.FromDate, message.ToDate, message.WeekFromDate, message.WeekToDate),
                betlistData,
                Builder,
                TicketHelper,
                showContributionAmountRoundNote,
                message.IsGroupHeadline,
                betlistOutputParams: betlistData.BetlistOutputParams);
        }

        public async Task<WinLossBetListWeekSelectionViewModel> Handle(WinLossBetListWeekSelectionQuery message)
        {
            var showContributionAmountRoundNote = message.IsEnabledJackpot;
            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var validUserSelectedProductIds = GetValidUserSelectedProductIds(products, message.UserSelectedProductIds);
            var validResourceIds = await productDetailService.FilterResourceIdsByCustomer(message.Site, message.CurrencyId, message.ResourceIds);
            var productDetailsXML = await productDetailService.BuildProductDetailsXMLAsync(validResourceIds, message.Is789Y);

            var winLossParam = new WinLossParam(
                message.CustId,
                validUserSelectedProductIds,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                message.Language,
                productDetailsXML,
                message.Site,
                message.MaxTotalRow,
                message.MaxTotalRowConfig);
            var betlistData = await winLossService.GetBetList(winLossParam);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossBetListWeekSelectionViewModel(
                message,
                betlistData,
                Builder,
                TicketHelper,
                showContributionAmountRoundNote,
                message.IsGroupHeadline,
                betlistOutputParams: betlistData.BetlistOutputParams);
        }

        public async Task<IEnumerable<WeekSelectionViewModel>> Handle(WinLossWeeksQuery message)
        {
            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var validUserSelectedProductIds = GetValidUserSelectedProductIds(products, message.UserSelectedProductIds);
            var validResourceIds = await productDetailService.FilterResourceIdsByCustomer(message.Site, message.CurrencyId, message.ResourceIds);
            var productDetailsXML = await productDetailService.BuildProductDetailsXMLAsync(validResourceIds, message.Is789Y);

            var winLossWeekDataParam = new WinLossWeekDataParam(
                message.ViewerId,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                validUserSelectedProductIds,
                productDetailsXML,
                message.CustName);
            var weekData = await winLossService.GetWinLossWeekData(winLossWeekDataParam);

            return BuildWeekSelectionViewModels(weekData, message.FromDate, message.ToDate);
        }

        public async Task<WinLossViewModel> Handle(WinLossBetListQuery message)
        {
            var showContributionAmountRoundNote = message.IsEnabledJackpot;
            var productsAndProductDetails = await GetProductsAndProductDetails(message);
            var products = productsAndProductDetails.Item1;
            var productDetails = productsAndProductDetails.Item2;

            var validUserSelectedProductIds = GetValidUserSelectedProductIds(products, message.UserSelectedProductIds);
            var validResourceIds = await productDetailService.FilterResourceIdsByCustomer(message.Site, message.CurrencyId, message.ResourceIds);

            var winLossParam = new WinLossParam(
                message.CustId,
                validUserSelectedProductIds,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                message.Language,
                await productDetailService.BuildProductDetailsXMLAsync(validResourceIds, message.Is789Y),
                message.Site,
                message.MaxTotalRow,
                message.MaxTotalRowConfig);

            var betlistData = await winLossService.GetBetList(winLossParam);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

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
            WinLossBetListWeekSelectionQuery message)
        {
            var products = productService.GetAllOfferedProducts(
                message.Site,
                message.CurrencyId,
                message.MinDate);
            var productDetails = await productDetailService.GetProductDetailsAsync(
                message.Site,
                message.CurrencyId);

            return Tuple.Create(products, productDetails);
        }

        private static IEnumerable<int> GetValidUserSelectedProductIds(
            IEnumerable<Product> products,
            IDictionary<int, bool> userSelectedProductIds)
            => products
            .Where(product => userSelectedProductIds.ContainsKey(product.Id))
            .Select(product => product.Id);
    }
}