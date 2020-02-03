namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Services;
    using SmartPunter.Services;
    using System;
    using System.Threading.Tasks;
    using Utilities.IP;
    using ViewModels.WinLoss;

    public class RiskAnalysisIPMatchingHandler : BetListBaseHandler, IAsyncRequestHandler<RiskAnalysisIPMatchingQuery, WinLossViewModel>
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

        public async Task<WinLossViewModel> Handle(RiskAnalysisIPMatchingQuery message)
        {
            var param = CreateWinLossParam(message);
            var betlistData = await riskAnalysisIPMatchingService.GetBetList(param);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossViewModel(message, betlistData, Builder, TicketHelper, true);
        }

        protected internal RiskAnalysisIPMatchingParam CreateWinLossParam(RiskAnalysisIPMatchingQuery message)
            => new RiskAnalysisIPMatchingParam(
               836949, //message.CustId,
               message.Date,
               new DateTime(2020, 1, 14),//reportDateService.GetMoveBetsDay(),
               message.IP);
    }
}