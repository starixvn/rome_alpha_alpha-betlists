namespace Alpha.BetLists.Site.Requests.CombinationMixParlay
{
    using BetListByBetType;
    using BetLists.Constants;
    using BetLists.Models;
    using MediatR;
    using ViewModels.CombinationMixParlay;

    public class CombinationMixParlayQuery : BetListByBetTypeQuery, IRequest<CombinationMixParlayViewModel>
    {
        public CombinationMixParlayQuery(IUser viewer) : base(viewer)
        {
            BetType = BetTypeIdConstants.CombinationMixParlay;
        }
    }
}
