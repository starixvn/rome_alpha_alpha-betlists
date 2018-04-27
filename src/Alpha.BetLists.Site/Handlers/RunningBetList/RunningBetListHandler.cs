namespace Alpha.BetLists.Site.Handlers.RunningBetList
{
    using System.Threading.Tasks;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.RunningBetList;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.RunningBetList;

    public class RunningBetListHandler : BetListBaseHandler, IAsyncRequestHandler<RunningBetListQuery, RunningBetListViewModel>
    {
        private readonly RunningBetListService runningBetListService;
        private readonly ProductService productService;
        private readonly ForecastService forecastService;

        public RunningBetListHandler(
            RunningBetListService runningBetListService,
            ProductService productService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.runningBetListService = runningBetListService;
            this.productService = productService;
            this.forecastService = forecastService;
        }

        public async Task<RunningBetListViewModel> Handle(RunningBetListQuery message)
        {
            var validUserSelectedProductIds = productService.FilterProductIdsByCustomer(message.CustId, message.MinDate, message.ProductIds.ToListInt());

            var runningBetListParam = new RunningBetListParam(
                message.CustId,
                validUserSelectedProductIds,
                message.FromDate,
                message.ToDate,
                message.LimitedDateToShowInfo,
                message.GetAll,
                message.Language,
                message.CustName);

            var betlistData = await runningBetListService.GetBetList(runningBetListParam);
            var products = productService.GetProductsForCustomer(message.CustId, message.MinDate);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new RunningBetListViewModel(
                message,
                runningBetListParam,
                betlistData,
                BuildProductSelectListItems(runningBetListParam.ProductIds, products),
                supportScoreMapBetTypeIds,
                Builder,
                TicketHelper);
        }
    }
}
