namespace Alpha.BetLists.Site.Handlers.RacingByMatch
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.RacingByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.RacingByMatch;

    public class RacingByMatchHandler : BetListBaseHandler, IAsyncRequestHandler<RacingByMatchQuery, RacingByMatchViewModel>
    {
        private readonly ForecastService forecastService;
        private readonly RacingByMatchService racingByMatchService;

        public RacingByMatchHandler(
            RacingByMatchService racingByMatchService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.forecastService = forecastService;
            this.racingByMatchService = racingByMatchService;
        }

        public async Task<RacingByMatchViewModel> Handle(RacingByMatchQuery message)
        {
            var param = CreateRacingByMatchParam(message);

            var tickets = await racingByMatchService.GetBetList(param);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new RacingByMatchViewModel(
                message,
                new BetListData(tickets, null),
                Builder,
                TicketHelper,
                supportScoreMapBetTypeIds);
        }

        protected internal static RacingByMatchParam CreateRacingByMatchParam(RacingByMatchQuery message)
            => new RacingByMatchParam(
                message.ViewerId,
                message.BetType,
                message.MatchId,
                message.Language,
                message.ViewerLevel);
    }
}
