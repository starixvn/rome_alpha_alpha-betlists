namespace Alpha.BetLists.Site.Handlers
{
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Fanex.BetDetails.BetBuilder.Services;
    using Fanex.BetDetails.BetBuilder.ViewModels;
    using MediatR;
    using Requests.BetBuilder;

    public class BetBuilderHandler : IRequestHandler<BetBuilderDetailQuery, BetBuilderDetailViewModel>
    {
        private readonly IBetBuilderService betBuilderService;

        public BetBuilderHandler(IBetBuilderService service)
        {
            this.betBuilderService = service;
        }

        public BetBuilderDetailViewModel Handle(BetBuilderDetailQuery message)
        {
            var parameter = new GetBetBuilderDetailParameter
            {
                CustomerId = message.CustomerId,
                TransactionId = message.TransactionId,
                Status = message.Status
            };

            return betBuilderService.GetBetListDetail(parameter);
        }
    }
}