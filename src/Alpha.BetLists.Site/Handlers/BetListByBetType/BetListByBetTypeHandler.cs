namespace Alpha.BetLists.Site.Handlers.BetListByBetType
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.BetListByBetType;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.BetListByBetType;

    public class BetListByBetTypeHandler : BetListBaseHandler, IAsyncRequestHandler<BetListByBetTypeQuery, BetListByBetTypeViewModel>
    {
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;

        public BetListByBetTypeHandler(
            RunningBetListByBetTypeService runningBetListByBetTypeService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.forecastService = forecastService;
            this.runningBetListByBetTypeService = runningBetListByBetTypeService;
        }

        public async Task<BetListByBetTypeViewModel> Handle(BetListByBetTypeQuery message)
        {
            var param = CreateRunningBetListByBetTypeParam(message);

            var tickets = await runningBetListByBetTypeService.GetBetList(param);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new BetListByBetTypeViewModel(
                message,
                new BetListData(tickets, null),
                Builder,
                TicketHelper,
                supportScoreMapBetTypeIds);
        }

        protected internal static RunningBetListByBetTypeParam CreateRunningBetListByBetTypeParam(BetListByBetTypeQuery message)
            => new RunningBetListByBetTypeParam(
                message.ViewerId,
                message.BetType,
                message.Language,
                message.ViewerLevel);
    }
}
