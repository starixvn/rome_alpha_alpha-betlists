namespace Alpha.BetLists.Site.Requests.BetDetail
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.BetDetail;

    public class RNGCasinoDetailQuery : BetDetailQuery, IRequest<RNGCasinoDetailViewModel>
    {
        public RNGCasinoDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public string SportName { get; set; }
    }
}
