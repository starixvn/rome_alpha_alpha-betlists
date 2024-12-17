namespace Alpha.BetLists.Site.Requests.CasinoDetails
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.CasinoDetails;

    public class RNGCasinoDetailsQuery : CasinoDetailsQuery, IRequest<CasinoDetailsViewModel>
    {
        public RNGCasinoDetailsQuery(IUser viewer) : base(viewer)
        {
        }
    }
}
