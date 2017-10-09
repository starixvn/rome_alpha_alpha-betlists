namespace Alpha.BetLists.Site.Requests.CorrectScore
{
    using BetListByBetType;
    using BetLists.Constants;
    using BetLists.Models;
    using MediatR;
    using ViewModels.CorrectScore;

    public class CorrectScoreQuery : BetListByBetTypeQuery, IRequest<CorrectScoreViewModel>
    {
        public CorrectScoreQuery(IUser viewer) : base(viewer)
        {
            BetType = BetTypeIdConstants.CorrectScore;
        }
    }
}
