namespace Alpha.BetLists.Site.Requests.Lucky3
{
    using BetLists.Models;
    using Fanex.BetDetails.Lucky3.ViewModels;
    using MediatR;

    public class Lucky3DetailQuery : BaseQuery, IRequest<Lucky3DetailViewModel>
    {
        public Lucky3DetailQuery(IUser viewer) : base(viewer)
        {
        }

        public long TransactionId { get; set; }
    }
}