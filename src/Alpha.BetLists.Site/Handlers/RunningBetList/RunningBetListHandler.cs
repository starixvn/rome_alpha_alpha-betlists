namespace Alpha.BetLists.Site.Handlers.RunningBetList
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.RunningBetList;
    using Services;
    using System.Threading.Tasks;
    using ViewModels.RunningBetList;

    public class RunningBetListHandler : BetListBaseHandler, IAsyncRequestHandler<RunningBetListQuery, RunningBetListViewModel>
    {
        private readonly RunningBetListService runningBetListService;
        private readonly ProductService productService;
        private readonly ForecastService forecastService;
        private readonly AutoFollowBetService autoFollowBetService;

        public RunningBetListHandler(
            RunningBetListService runningBetListService,
            ProductService productService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            AutoFollowBetService autoFollowBetService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.runningBetListService = runningBetListService;
            this.productService = productService;
            this.forecastService = forecastService;
            this.autoFollowBetService = autoFollowBetService;
        }

        public async Task<RunningBetListViewModel> Handle(RunningBetListQuery message)
        {
            if (message.AutoFollowBetIsDummyAgent)
            {
                message.CustId = await autoFollowBetService.GetRealCustId(message.ViewerId, message.CustId);
            }

            var validUserSelectedProductIds = productService.FilterProductIds(
                message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt());

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
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);
            var products = productService.GetAllOfferedProducts(message.Site, message.CurrencyId, message.MinDate);
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
