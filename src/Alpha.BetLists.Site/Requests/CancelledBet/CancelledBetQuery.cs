using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels.CancelledBet;
using MediatR;

namespace Alpha.BetLists.Site.Requests.CancelledBet
{
    public class CancelledBetQuery : CancelledBetBaseQuery, IRequest<CancelledBetViewModel>
    {
        public CancelledBetQuery(IUser viewer) : base(viewer)
        {
        }
    }
}