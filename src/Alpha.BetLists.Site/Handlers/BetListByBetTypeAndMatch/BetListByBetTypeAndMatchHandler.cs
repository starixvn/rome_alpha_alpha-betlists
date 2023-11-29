namespace Alpha.BetLists.Site.Handlers.BetListByBetTypeAndMatch
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.BetListByBetTypeAndMatch;
    using Services;
    using System.Threading.Tasks;
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
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.forecastService = forecastService;
            this.runningBetListByBetTypeAndMatchService = runningBetListByBetTypeAndMatchService;
        }

        public async Task<BetListByBetTypeAndMatchViewModel> Handle(BetListByBetTypeAndMatchQuery message)
        {
            var param = CreateRunningBetListByBetTypeAndMatchParam(message);

            var betListData = await runningBetListByBetTypeAndMatchService.GetBetList(param);
            await AssignCountry(betListData.Tickets, message.DisplayVNIP);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betListData.Tickets);

            return new BetListByBetTypeAndMatchViewModel(
                message,
                betListData,
                Builder,
                TicketHelper,
                supportScoreMapBetTypeIds);
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
