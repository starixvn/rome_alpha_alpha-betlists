namespace Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Services;
    using System.Threading.Tasks;

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
            RiskAnalysisIPMatchingHelper riskAnalysisIPMatchingHelper,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.riskAnalysisIPMatchingHelper = riskAnalysisIPMatchingHelper;
        }

        public async Task<RiskAnalysisIPMatchingViewModel> Handle(RiskAnalysisIPMatchingQuery message)
        {
            var betlistData = await riskAnalysisIPMatchingHelper.BuildRiskAnalysisIPMatchingViewModel(message);

            if (betlistData != null)
            {
                await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

                return new RiskAnalysisIPMatchingViewModel(message, betlistData, Builder, TicketHelper);
            }

            return null;
        }
    }
}