namespace Alpha.BetLists.Site.Requests.WinLoss
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.WinLoss;

    public class WinLossQuery : BaseQuery, IRequest<WinLossViewModel>
    {
        public WinLossQuery(IUser viewer) : base(viewer)
        {
        }
    }
}