using Alpha.BetLists.Models;
using Alpha.BetLists.Site.ViewModels.CancelledBet;
using MediatR;

namespace Alpha.BetLists.Site.Requests.CancelledBet
{
    public class CancelledBetBaseQuery : BaseQuery, IRequest<CancelledBetViewModel>
    {
        protected CancelledBetBaseQuery(IUser viewer) : base(viewer)
        {
        }

        public int MasterId { get; set; }

        public int AgentId { get; set; }

        public int MemberId { get; set; }

    }
}