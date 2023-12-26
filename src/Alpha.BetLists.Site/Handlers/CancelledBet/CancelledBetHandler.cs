namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using System.Linq;
    using System.Threading.Tasks;

    public class CancelledBetHandler : BetListBaseHandler, IAsyncRequestHandler<CancelledBetQuery, CancelledBetViewModel>
    {
        private readonly CancelledBetService service;
        private readonly ProductService productService;
        private readonly ReportDateService reportDateService;

        public CancelledBetHandler(
            CancelledBetService service,
            ProductService productService,
            ReportDateService reportDateService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.service = service;
            this.productService = productService;
            this.reportDateService = reportDateService;
        }

        public async Task<CancelledBetViewModel> Handle(CancelledBetQuery message)
        {
            var validUserSelectedProductIds = productService.GetProductsHaveSupportCancelledBet(message.ViewerId, 
                message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt());
            var allProducts = productService.GetProductsHaveSupportCancelledBet(message.ViewerId, 
                message.Site, message.CurrencyId, message.MinDate);
            var parameter = new CancelledBetParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                reportDateService.GetMoveBetsDay(),
                message.ViewerLevelId,
                validUserSelectedProductIds.Select(i => i.Id),
                message.StatusIds,
                message.Language,
                message.PageSize,
                message.PageIndex);
            var cancelledBet = await service.GetCancelledBet(parameter);
            var betlistData = cancelledBet.DataSource.FirstOrDefault();
            if (betlistData != null)
            {
                await AssignCountry(betlistData.Tickets, message.DisplayVNIP);
            }

            return new CancelledBetViewModel(
                message,
                parameter,
                cancelledBet,
                BuildProductSelectListItems(parameter.ProductIds, allProducts),
                Builder,
                TicketHelper);
        }
    }
}