namespace Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using AlphaPlus.Utilities.IP;
    using Params;
    using Services;
    using System.Threading.Tasks;

    public interface RiskAnalysisIPMatchingHelper
    {
        Task<BetListData> BuildRiskAnalysisIPMatchingViewModel(RiskAnalysisIPMatchingQuery message);
    }

    public class RiskAnalysisIPMatchingHelperImpl : RiskAnalysisIPMatchingHelper
    {
        private readonly RiskAnalysisIPMatchingService riskAnalysisIPMatchingService;
        private readonly IPService ipService;
        private readonly ReportDateService reportDateService;

        public RiskAnalysisIPMatchingHelperImpl(
            RiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
            ReportDateService reportDateService,
            IPService ipService)
        {
            this.riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;
            this.ipService = ipService;
            this.reportDateService = reportDateService;
        }

        public async Task<BetListData> BuildRiskAnalysisIPMatchingViewModel(RiskAnalysisIPMatchingQuery message)
        {
            if (message.IsSuperLevel && (message.DisplayVNIP || !ipService.IsVNIP(message.IP)))
            {
                var param = new RiskAnalysisIPMatchingParam(
                       message.CustId,
                       message.Date,
                       reportDateService.GetMoveBetsDay(),
                       message.IP);

                return await riskAnalysisIPMatchingService.GetBetList(param);
            }

            return null;
        }
    }
}