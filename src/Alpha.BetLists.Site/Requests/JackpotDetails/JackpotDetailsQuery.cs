namespace Alpha.BetLists.Site.Requests.JackpotDetails
{
    using BetLists.Models;
    using MediatR;
    using ViewModels.JackpotDetails;

    public class JackpotDetailsQuery : BaseQuery, IRequest<JackpotDetailsViewModel>
    {
        public JackpotDetailsQuery(IUser viewer)
            : base(viewer)
        {
        }

        public long TransId { get; set; }

        public int BetType { get; set; }
    }
}