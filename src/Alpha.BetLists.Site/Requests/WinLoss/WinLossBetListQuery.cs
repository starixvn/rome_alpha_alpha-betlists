namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using MediatR;

    public class WinLossBetListQuery : WinLossBetListWeekSelectionQuery, IRequest<WinLossViewModel>
    {
        public WinLossBetListQuery(IUser viewer) : base(viewer)
        {
        }
    }
}