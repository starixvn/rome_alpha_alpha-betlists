namespace Alpha.BetLists.Site.Requests.BetDetail
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.BetDetail;

    public class AllbetDetailQuery : BetDetailQuery, IRequest<AllbetDetailViewModel>
    {
        public AllbetDetailQuery(IUser viewer) : base(viewer)
        {
        }
    }
}
