namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.WinLoss;
    using MediatR;

    public class WinLossNewQuery : WinLossGetDataQuery, IRequest<WinLossNewViewModel>
    {
        public WinLossNewQuery(IUser viewer) : base(viewer)
        {
        }
    }
}