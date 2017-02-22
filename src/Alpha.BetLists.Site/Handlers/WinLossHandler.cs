namespace Alpha.BetLists.Site.Handlers
{
    using Extensions;
    using Fanex.BetList.Core.Factories;
    using MediatR;
    using Params;
    using Requests.WinLoss;
    using Services;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        IRequestHandler<WinLossQuery, WinLossViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly IBetListFactory betListFactory;

        public WinLossHandler(WinLossService winLossService)
        {
            this.winLossService = winLossService;
        }

        public WinLossViewModel Handle(WinLossQuery message)
        {
            var winLossParam = new WinLossParam(
                message.CustId,
                message.ProductIds.ToListInt(),
                message.FromDate, message.ToDate,
                message.Language);

            var betlistData = winLossService.GetBetList(winLossParam);

            return new WinLossViewModel(message, betlistData);
        }
    }
}