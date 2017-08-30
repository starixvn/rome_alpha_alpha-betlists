namespace Alpha.BetLists.Site.Handlers.BetListByBetTypeAndMatch
{
    using System.Threading.Tasks;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.BetListByBetTypeAndMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.BetListByBetTypeAndMatch;

    public class BetListByBetTypeAndMatchHandler : BetListBaseHandler,
        IAsyncRequestHandler<BetListByBetTypeAndMatchQuery, BetListByBetTypeAndMatchViewModel>
    {
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService;

        public BetListByBetTypeAndMatchHandler(
            RunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.forecastService = forecastService;
            this.runningBetListByBetTypeAndMatchService = runningBetListByBetTypeAndMatchService;
        }

        public async Task<BetListByBetTypeAndMatchViewModel> Handle(BetListByBetTypeAndMatchQuery message)
        {
            var param = CreateRunningBetListByBetTypeAndMatchParam(message);

            var betListData = await runningBetListByBetTypeAndMatchService.GetBetList(param);
            var supportScoreMapBettypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betListData.Tickets);

            return new BetListByBetTypeAndMatchViewModel(
                message,
                betListData,
                Builder,
                TicketHelper,
                supportScoreMapBettypeIds);
        }

        protected internal static RunningBetListByBetTypeAndMatchParam CreateRunningBetListByBetTypeAndMatchParam(BetListByBetTypeAndMatchQuery message)
            => new RunningBetListByBetTypeAndMatchParam(
                message.MatchId,
                message.ViewerId,
                message.BetType,
                message.Language,
                message.IsLive,
                message.BetTeam,
                message.ViewerLevel);
    }
}
