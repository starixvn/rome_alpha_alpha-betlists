namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Models;
    using MediatR;

    public class BookedBetQuery : BookedBetBaseQuery, IRequest<BookedBetViewModel>
    {
        public BookedBetQuery(IUser viewer) : base(viewer)
        {
        }        
    }
}