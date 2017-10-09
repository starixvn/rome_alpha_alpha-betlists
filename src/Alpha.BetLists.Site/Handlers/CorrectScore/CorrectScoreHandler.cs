namespace Alpha.BetLists.Site.Handlers.CorrectScore
{
    using System.Threading.Tasks;
    using BetLists.Models;
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

            var tickets = await correctScoreService.GetBetList(correctScoreParam);
            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new CorrectScoreViewModel(message, new BetListData(tickets, null), Builder, TicketHelper, supportScoreMapBetTypeIds);
        }

        protected internal static CorrectScoreParam CreateCorrectScoreParam(CorrectScoreQuery message)
            => new CorrectScoreParam(
                message.ViewerId,
                message.Language,
                message.ViewerLevel);
    }
}
