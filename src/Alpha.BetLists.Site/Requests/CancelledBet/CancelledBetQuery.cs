namespace Alpha.BetLists.Site.Requests.CancelledBet
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using MediatR;

    public class CancelledBetQuery : CancelledBetBaseQuery, IRequest<CancelledBetViewModel>
    {
        public CancelledBetQuery(IUser viewer) : base(viewer)
        {
        }
    }
}