namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Models;
    using MediatR;

    public class BookedBetDataQuery : BookedBetBaseQuery, IRequest<BookedBetDataViewModel>
    {
        public BookedBetDataQuery(IUser viewer) : base(viewer)
        {
        }
    }
}