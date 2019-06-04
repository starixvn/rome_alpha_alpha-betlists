namespace Alpha.BetLists.Site.Requests.BetBuilder
{
    using Fanex.BetDetails.BetBuilder.ViewModels;
    using MediatR;

    public class BetBuilderDetailQuery : IRequest<BetBuilderDetailViewModel>
    {
        public long TransactionId { get; set; }

        public int CustomerId { get; set; }

        public string Status { get; set; }
    }
}