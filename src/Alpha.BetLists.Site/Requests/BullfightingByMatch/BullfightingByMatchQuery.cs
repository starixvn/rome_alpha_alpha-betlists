namespace Alpha.BetLists.Site.Requests.BullfightingByMatch
{
    using BetListByBetType;
    using BetLists.Models;
    using MediatR;
    using ViewModels.BullfightingByMatch;

    public class BullfightingByMatchQuery : BetListByBetTypeQuery, IRequest<BullfightingByMatchViewModel>
    {
        public BullfightingByMatchQuery(IUser viewer) : base(viewer)
        {
        }

        public int MatchId { get; set; }
    }
}
