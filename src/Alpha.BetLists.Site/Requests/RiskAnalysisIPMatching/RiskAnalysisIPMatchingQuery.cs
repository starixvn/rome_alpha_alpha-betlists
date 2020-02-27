namespace Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using BetLists.Models;
    using MediatR;
    using System;

    public class RiskAnalysisIPMatchingQuery : BaseQuery, IRequest<RiskAnalysisIPMatchingViewModel>
    {
        public RiskAnalysisIPMatchingQuery(IUser viewer) : base(viewer)
        {
            IsSuperLevel = ViewerLevel.IsSuperLevel();
        }

        public DateTime Date { get; set; }

        public string IP { get; set; }

        public bool IsSuperLevel { get; }
    }
}