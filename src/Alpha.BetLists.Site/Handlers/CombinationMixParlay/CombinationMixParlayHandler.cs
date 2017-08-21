namespace Alpha.BetLists.Site.Handlers.CombinationMixParlay
{
    using System.Threading.Tasks;
    using BetListByBetType;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.CombinationMixParlay;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.CombinationMixParlay;

    public class CombinationMixParlayHandler : BetListBaseHandler,
        IAsyncRequestHandler<CombinationMixParlayQuery, CombinationMixParlayViewModel>
    {
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;

        public CombinationMixParlayHandler(
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

        public async Task<CombinationMixParlayViewModel> Handle(CombinationMixParlayQuery message)
        {
            var param = BetListByBetTypeHandler.CreateRunningBetListByBetTypeParam(message);

            var betListData = await runningBetListByBetTypeService.GetBetList(param);
            var supportScoreMapBettypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betListData.Tickets);

            return new CombinationMixParlayViewModel(
                message,
                betListData,
                Builder,
                TicketHelper,
                supportScoreMapBettypeIds);
        }
    }
}
