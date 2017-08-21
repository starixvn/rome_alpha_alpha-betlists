namespace Alpha.BetLists.Site.Handlers.CorrectScore
{
    using System.Threading.Tasks;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.CorrectScore;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.CorrectScore;

    public class CorrectScoreHandler : BetListBaseHandler, IAsyncRequestHandler<CorrectScoreQuery, CorrectScoreViewModel>
    {
        private readonly CorrectScoreService correctScoreService;
        private readonly ForecastService forecastService;

        public CorrectScoreHandler(
            CorrectScoreService correctScoreService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.correctScoreService = correctScoreService;
            this.forecastService = forecastService;
        }

        public async Task<CorrectScoreViewModel> Handle(CorrectScoreQuery message)
        {
            var correctScoreParam = CreateCorrectScoreParam(message);

            var betListData = await correctScoreService.GetBetList(correctScoreParam);
            var supportScoreMapBettypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betListData.Tickets);

            return new CorrectScoreViewModel(message, betListData, Builder, TicketHelper, supportScoreMapBettypeIds);
        }

        protected internal static CorrectScoreParam CreateCorrectScoreParam(CorrectScoreQuery message)
            => new CorrectScoreParam(
                message.ViewerId,
                message.Language,
                message.ViewerLevel);
    }
}
