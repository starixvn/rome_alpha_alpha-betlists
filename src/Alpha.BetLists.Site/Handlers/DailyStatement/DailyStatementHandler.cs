namespace Alpha.BetLists.Site.Handlers.DailyStatement
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.DailyStatement;
    using Services;
    using System.Threading.Tasks;
    using ViewModels.DailyStatement;

    public class DailyStatementHandler : BetListBaseHandler, IAsyncRequestHandler<DailyStatementQuery, DailyStatementViewModel>
    {
        private readonly DailyStatementService dailyStatementService;
        private readonly ForecastService forecastService;

        public DailyStatementHandler(
            DailyStatementService dailyStatementService,
            ForecastService forecastService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.dailyStatementService = dailyStatementService;
            this.forecastService = forecastService;
        }

        public async Task<DailyStatementViewModel> Handle(DailyStatementQuery message)
        {
            var param = CreateDailyStatementParam(message);
            var betlistData = await dailyStatementService.GetBetList(param);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            var supportScoreMapBetTypeIds = forecastService.GetBetTypeIdsSupportScoreMap();

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new DailyStatementViewModel(message, betlistData, Builder, TicketHelper, supportScoreMapBetTypeIds);
        }

        protected internal static DailyStatementParam CreateDailyStatementParam(DailyStatementQuery message)
            => new DailyStatementParam(message.CustId, message.WinLossDate, message.TransType, message.Language);
    }
}
