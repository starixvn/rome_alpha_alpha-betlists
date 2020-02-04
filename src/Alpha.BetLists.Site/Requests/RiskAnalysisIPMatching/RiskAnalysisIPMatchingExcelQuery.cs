namespace Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ViewModels;

    public class RiskAnalysisIPMatchingExcelQuery : RiskAnalysisIPMatchingQuery, IRequest<ExcelFileViewModel>
    {
        public RiskAnalysisIPMatchingExcelQuery(IUser viewer) : base(viewer)
        {
        }

        /// <summary>
        /// Use for unit testing
        /// </summary>
        public RTFHelper? RtfHelper { get; set; }
    }
}
