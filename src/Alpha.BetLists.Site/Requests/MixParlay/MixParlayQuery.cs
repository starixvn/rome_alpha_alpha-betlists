namespace Alpha.BetLists.Site.Requests.MixParlay
{
    using BetListByBetType;
    using BetLists.Constants;
    using BetLists.Models;
    using MediatR;
    using ViewModels.MixParlay;

    public class MixParlayQuery : BetListByBetTypeQuery, IRequest<MixParlayViewModel>
    {
        public MixParlayQuery(IUser viewer) : base(viewer)
        {
            BetType = BetTypeIdConstants.MixParlay;
        }
    }
}
