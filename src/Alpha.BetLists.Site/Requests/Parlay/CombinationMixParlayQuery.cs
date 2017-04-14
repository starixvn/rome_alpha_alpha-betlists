namespace Alpha.BetLists.Site.Requests.Parlay
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.Parlay;

    public class CombinationMixParlayQuery : BaseQuery, IRequest<CombinationMixParlayViewModel>
    {
        public CombinationMixParlayQuery(IUser viewer) : base(viewer)
        {
        }

        public long TransId { get; set; }

        public decimal Handicap1 { get; set; }

        public decimal Handicap2 { get; set; }

        public string BetCheck { get; set; }

        public string BetTeam { get; set; }

        public string Language { get; set; }
    }
}