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

        public WinLossHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            ProductService productService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.winLossService = winLossService;
            this.productService = productService;
        }

        public async Task<WinLossViewModel> Handle(WinLossQuery message)
        {
            var validUserSelectedProductIds = message.IsGroupHeadline
                ? productService.FilterProductIdsByGroupHeadline(message.ProductIds.ToListInt(), new GetProductsByGroupHeadlineParam(message.ViewerId, message.MinDate))
                : productService.FilterProductIdsByCustomer(message.CustId, message.MinDate, message.ProductIds.ToListInt());

            var showContributionAmountRoundNote = message.IsGroupHeadline
                ? productService.IsEnabledJackpot(message.CustId)
                : message.IsEnabledJackpot;

            var winLossParam = CreateWinLossParam(message, validUserSelectedProductIds);
            var betlistData = await winLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossViewModel(message, betlistData, Builder, TicketHelper, showContributionAmountRoundNote);
        }

        protected internal static WinLossParam CreateWinLossParam(WinLossQuery message, IEnumerable<int> productIds)
            => new WinLossParam(
               message.CustId,
               productIds,
               message.FromDate,
               message.ToDate,
               message.Language);
    }
}