namespace Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using BetLists.Models;
    using MediatR;
    using System;
    using ViewModels.WinLoss;

    public class RiskAnalysisIPMatchingQuery : BaseQuery, IRequest<RiskAnalysisIPMatchingViewModel>
    {
        public RiskAnalysisIPMatchingQuery(IUser viewer) : base(viewer)
        {
        }

        public DateTime Date { get; set; }

        public string IP { get; set; }

        public override bool IsValid() => ViewerLevel.IsSuperLevel();
    }
}