using System.Linq;
using System.Threading.Tasks;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels.WinLoss;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    public class WinLossByMatchHandler : WinLossByMatchBaseHandler, IAsyncRequestHandler<WinLossByMatchQuery, WinLossViewModel>
    {
        private readonly WinLossByMatchService winLossService;
        private readonly ISmartPunterBetListMonitor monitorService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;
        private readonly IPService ipService;

        public WinLossByMatchHandler(
            WinLossByMatchService winLossService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IPService ipService)
        {
            this.winLossService = winLossService;

            this.monitorService = monitorService;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
            this.ipService = ipService;
        }

        public async Task<WinLossViewModel> Handle(WinLossByMatchQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await winLossService.GetBetList(winLossParam);

            monitorService.WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets.ToList());

            ticketHelper.IsVnIp = ipService.IsVNIP;

            return new WinLossViewModel(message, betlistData, builder, ticketHelper);
        }
    }
}