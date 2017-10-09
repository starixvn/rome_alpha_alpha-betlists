namespace Alpha.BetLists.Site.Requests.RacingByMatch
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.RacingByMatch;

    public class RacingByMatchQuery : BaseQuery, IRequest<RacingByMatchViewModel>
    {
        public RacingByMatchQuery(IUser viewer) : base(viewer)
        {
        }

        public int BetType { get; set; }

        public int MatchId { get; set; }
    }
}
