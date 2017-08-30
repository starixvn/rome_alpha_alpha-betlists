namespace Alpha.BetLists.Site.Requests.RacingByMatch
{
    using BetListByBetType;
    using BetLists.Models;
    using MediatR;
    using ViewModels.RacingByMatch;

    public class RacingByMatchQuery : BetListByBetTypeQuery, IRequest<RacingByMatchViewModel>
    {
        public RacingByMatchQuery(IUser viewer) : base(viewer)
        {
            IsShowRacing = viewer.IsShowRacing;
        }

        public int MatchId { get; set; }

        public bool IsShowRacing { get; }
    }
}
