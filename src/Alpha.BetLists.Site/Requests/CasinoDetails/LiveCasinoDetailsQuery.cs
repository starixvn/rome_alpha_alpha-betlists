namespace Alpha.BetLists.Site.Requests.CasinoDetails
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.CasinoDetails;

    public class LiveCasinoDetailsQuery : CasinoDetailsQuery, IRequest<CasinoDetailsViewModel>
    {
        public LiveCasinoDetailsQuery(IUser viewer) : base(viewer)
        {
        }
    }
}
