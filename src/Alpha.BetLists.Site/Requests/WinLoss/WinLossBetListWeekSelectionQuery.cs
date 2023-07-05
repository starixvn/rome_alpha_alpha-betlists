namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using MediatR;

    public class WinLossBetListWeekSelectionQuery : WinLossBetListQuery, IRequest<WinLossBetListWeekSelectionViewModel>
    {
        public WinLossBetListWeekSelectionQuery(IUser viewer) : base(viewer)
        {
        }

        public string BreadcrumbLevelJson { get; set; }
    }
}