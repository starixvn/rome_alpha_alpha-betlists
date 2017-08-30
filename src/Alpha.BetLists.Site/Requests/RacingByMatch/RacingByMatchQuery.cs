namespace Alpha.BetLists.Site.Requests.RacingByMatch
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.RacingByMatch;

    public class RacingByMatchQuery : BaseQuery, IRequest<RacingByMatchViewModel>
    {
        public RacingByMatchQuery(IUser viewer) : base(viewer)
        {
            IsShowRacing = viewer.IsShowRacing;
        }

        public int BetType { get; set; }

        public int MatchId { get; set; }

        public bool IsShowRacing { get; }
    }
}
