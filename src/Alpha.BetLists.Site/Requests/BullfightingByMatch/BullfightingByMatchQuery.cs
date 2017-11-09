namespace Alpha.BetLists.Site.Requests.BullfightingByMatch
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.BullfightingByMatch;

    public class BullfightingByMatchQuery : BaseQuery, IRequest<BullfightingByMatchViewModel>
    {
        public BullfightingByMatchQuery(IUser viewer) : base(viewer)
        {
        }        

        public int MatchId { get; set; }
    }
}
