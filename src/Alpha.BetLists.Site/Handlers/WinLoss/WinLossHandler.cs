namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.Logging;
    using MediatR;
    using Params;
    using Requests.WinLoss;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        BetListBaseHandler,
        IAsyncRequestHandler<WinLossQuery, WinLossViewModel>
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
            ProductDetailService productDetailService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.winLossService = winLossService;
            this.productService = productService;
            this.productDetailService = productDetailService;
        }

        public async Task<WinLossViewModel> Handle(WinLossQuery message)
        {
            var productByHLParam = new GetProductsByGroupHeadlineParam(message.ViewerId, message.MinDate);
            var validUserSelectedProductIds = message.IsGroupHeadline
                ? productService.FilterProductIdsByGroupHeadline(message.ProductIds.ToListInt(), productByHLParam)
                : productService.FilterProductIds(message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt());
            var validResourceIds = await productDetailService.FilterResourceIdsByCustomer(message.Site, message.CurrencyId, message.ResourceIds);
            var productDetailsXML = await productDetailService.BuildProductDetailsXMLAsync(validResourceIds, message.Is789Y);

            var showContributionAmountRoundNote = message.IsGroupHeadline
                ? productService.IsEnabledJackpot(message.Site, message.CurrencyId)
                : message.IsEnabledJackpot;

            if (message.IsGroupHeadline && !validUserSelectedProductIds.Any())
            {
                await Logger.Log.ErrorAsync("validUserSelectedProductIds: " + string.Join(", ", validUserSelectedProductIds));
            }

            var winLossParam = CreateWinLossParam(message, validUserSelectedProductIds, productDetailsXML, message.MaxTotalRow, message.MaxTotalRowConfig);
            var betlistData = await winLossService.GetBetList(winLossParam);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

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