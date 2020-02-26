namespace Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Services;
    using SmartPunter.Services;
    using System.Threading.Tasks;
    using Utilities.IP;

    public class RiskAnalysisIPMatchingHandler :
        BetListBaseHandler, 
        IAsyncRequestHandler<RiskAnalysisIPMatchingQuery, RiskAnalysisIPMatchingViewModel>
    {
        private readonly RiskAnalysisIPMatchingHelper riskAnalysisIPMatchingHelper;

        public RiskAnalysisIPMatchingHandler(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            ReportDateService reportDateService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            RiskAnalysisIPMatchingHelper riskAnalysisIPMatchingHelper)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.riskAnalysisIPMatchingHelper = riskAnalysisIPMatchingHelper;
        }

        public async Task<RiskAnalysisIPMatchingViewModel> Handle(RiskAnalysisIPMatchingQuery message)
        {
            var betlistData = await riskAnalysisIPMatchingHelper.BuildRiskAnalysisIPMatchingViewModel(message);
            return betlistData == null ? null : new RiskAnalysisIPMatchingViewModel(message, betlistData, Builder, TicketHelper);
        }
    }
}