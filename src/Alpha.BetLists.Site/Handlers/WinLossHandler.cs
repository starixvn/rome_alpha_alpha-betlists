namespace Alpha.BetLists.Site.Handlers
{
    using System.Threading.Tasks;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using MediatR;
    using Params;
    using Requests.WinLoss;
    using Services;
    using ViewModels.WinLoss;

    public class WinLossHandler :
        IAsyncRequestHandler<WinLossQuery, WinLossViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;

        public WinLossHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            this.winLossService = winLossService;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
        }

        public async Task<WinLossViewModel> Handle(WinLossQuery message)
        {
            var winLossParam = new WinLossParam(
                message.CustId,
                message.ProductIds.ToListInt(),
                message.FromDate,
                message.ToDate,
                message.Language);

            var betlistData = await winLossService.GetBetList(winLossParam);

            return new WinLossViewModel(message, betlistData, builder, ticketHelper);
        }
    }
}