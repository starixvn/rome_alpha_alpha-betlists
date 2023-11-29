namespace Alpha.BetLists.Site.Handlers.CorrectScore
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.CorrectScore;
    using Services;
    using System.Threading.Tasks;
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
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.correctScoreService = correctScoreService;
            this.forecastService = forecastService;
        }

        public async Task<CorrectScoreViewModel> Handle(CorrectScoreQuery message)
        {
            var correctScoreParam = CreateCorrectScoreParam(message);

            var tickets = await correctScoreService.GetBetList(correctScoreParam);
            await AssignCountry(tickets, message.DisplayVNIP);
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
