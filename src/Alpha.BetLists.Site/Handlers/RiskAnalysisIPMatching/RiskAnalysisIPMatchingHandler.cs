namespace Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Services;
    using SmartPunter.Services;
    using System.Threading.Tasks;
    using Utilities.IP;

    public class RiskAnalysisIPMatchingHandler : BetListBaseHandler, IAsyncRequestHandler<RiskAnalysisIPMatchingQuery, RiskAnalysisIPMatchingViewModel>
    {
        private readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;
        private readonly ReportDateService reportDateService;

        public RiskAnalysisIPMatchingHandler(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            ReportDateService reportDateService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;
            this.reportDateService = reportDateService;
        }

        public async Task<RiskAnalysisIPMatchingViewModel> Handle(RiskAnalysisIPMatchingQuery message)
        {
            var param = new RiskAnalysisIPMatchingParam(
               message.CustId,
               message.Date,
               reportDateService.GetMoveBetsDay(),
               message.IP);

            var betlistData = await riskAnalysisIPMatchingService.GetBetList(param);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new RiskAnalysisIPMatchingViewModel(message, betlistData, Builder, TicketHelper, message.Date, message.IP);
        }
    }
}