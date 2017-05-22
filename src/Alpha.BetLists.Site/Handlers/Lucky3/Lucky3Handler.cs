namespace Alpha.BetLists.Site.Handlers
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.Services;
    using Fanex.BetListDetail.Lucky3.ViewModels;
    using MediatR;
    using Requests.Lucky3;

    public class Lucky3Handler : IRequestHandler<Lucky3DetailQuery, Lucky3DetailViewModel>
    {
        private readonly ILucky3Service lucky3Service;

        public Lucky3Handler(ILucky3Service lucky3Service)
        {
            this.lucky3Service = lucky3Service;
        }

        public Lucky3DetailViewModel Handle(Lucky3DetailQuery message)
        {
            var lucky3DetailParameter = new GetLucky3DetailParameter
            {
                CustomerId = message.CustId,
                TransactionId = message.TransactionId
            };

            var lucky3Details = lucky3Service.GetBetListDetail(lucky3DetailParameter);

            return new Lucky3DetailViewModel(lucky3Details, message.ViewerLevelId);
        }
    }
}