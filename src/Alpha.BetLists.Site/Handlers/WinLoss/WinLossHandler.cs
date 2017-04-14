namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.WinLoss;

    public class WinLossHandler : WinLossBaseHandler, IAsyncRequestHandler<WinLossQuery, WinLossViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;
        private readonly ISmartPunterBetListMonitor monitorService;
        private readonly IPService ipService;

        public WinLossHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
        {
            this.winLossService = winLossService;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
            this.monitorService = monitorService;
            this.ipService = ipService;
        }

        public async Task<WinLossViewModel> Handle(WinLossQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await winLossService.GetBetList(winLossParam);

            monitorService.WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets.ToList());

            ticketHelper.IsVnIp = ipService.IsVNIP;

            return new WinLossViewModel(message, betlistData, builder, ticketHelper);
        }
    }
}