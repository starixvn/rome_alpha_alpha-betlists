namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    using System.Threading.Tasks;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.Utilities.IP;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using SmartPunter.Services;

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
            IPService ipService) : base(builder, ticketHelper, monitorService, ipService)
        {
            this.service = service;
            this.productService = productService;
            this.reportDateService = reportDateService;
        }

        public async Task<CancelledBetViewModel> Handle(CancelledBetQuery message)
        {
            var validUserSelectedProductIds = productService.GetProductsHaveSupportCancelledBet(message.CustId, message.MinDate);
            var products = productService.GetProductsForCustomer(message.CustId, message.MinDate);
            var parameter = new CancelledBetParam(
                message.CustId, 
                message.FromDate, 
                message.ToDate, 
                reportDateService.GetMoveBetsDay(), 
                message.ViewerLevelId, 
                validUserSelectedProductIds, 
                message.StatusIds, 
                message.Language,
                message.PageSize,
                message.PageIndex);
            var cancelledBet = await service.GetCancelledBet(parameter);

            return new CancelledBetViewModel(
                message,
                parameter,
                cancelledBet,
                BuildProductSelectListItems(parameter.ProductIds, products),
                Builder,
                TicketHelper);
        }
    }
}