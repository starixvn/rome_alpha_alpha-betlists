namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Sunplus.Agent.Model.Base;
    using ViewModels.WinLoss;

    public class WinLossHandler : WinLossBaseHandler, IAsyncRequestHandler<WinLossQuery, WinLossViewModel>
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
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await winLossService.GetBetList(winLossParam);

            return new WinLossViewModel(message, betlistData, builder, ticketHelper);
        }
    }
}