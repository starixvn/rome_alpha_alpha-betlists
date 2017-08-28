namespace Alpha.BetLists.Site.Requests.BetListByBetTypeAndMatch
{
    using BetListByBetType;
    using BetLists.Models;
    using MediatR;
    using ViewModels.BetListByBetTypeAndMatch;

    public class BetListByBetTypeAndMatchQuery : BetListByBetTypeQuery, IRequest<BetListByBetTypeAndMatchViewModel>
    {
        public BetListByBetTypeAndMatchQuery(IUser viewer) : base(viewer)
        {
            IsShowRacing = viewer.IsShowRacing;
        }

        public int MatchId { get; set; }

        public int IsLive { get; set; }

        public string BetTeam { get; set; }

        public bool IsShowRacing { get; }
    }
}
