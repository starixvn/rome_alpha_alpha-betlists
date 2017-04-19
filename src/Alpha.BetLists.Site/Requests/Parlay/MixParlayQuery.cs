namespace Alpha.BetLists.Site.Requests.Parlay
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.Parlay;

    public class MixParlayQuery : BaseQuery, IRequest<MixParlayViewModel>
    {
        public MixParlayQuery(IUser viewer) : base(viewer)
        {
        }

        public long TransId { get; set; }
    }
}