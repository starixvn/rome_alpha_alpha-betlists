namespace Alpha.BetLists.Site.Requests.BetListByBetType
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.BetListByBetType;

    public class BetListByBetTypeQuery : BaseQuery, IRequest<BetListByBetTypeViewModel>
    {
        public BetListByBetTypeQuery(IUser viewer) : base(viewer)
        {
        }

        public int BetType { get; set; }
    }
}
