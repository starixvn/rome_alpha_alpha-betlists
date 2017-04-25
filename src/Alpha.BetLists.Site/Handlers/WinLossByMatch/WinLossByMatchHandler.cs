using System.Threading.Tasks;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels.WinLossByMatch;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    public class WinLossByMatchHandler : WinLossByMatchBaseHandler, IAsyncRequestHandler<WinLossByMatchQuery, WinLossByMatchViewModel>
    {
        public WinLossByMatchHandler(
            WinLossByMatchService winLossService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService)
            : base(winLossService, monitorService, builder, excelBuilder, ticketHelper, ipService)
        {
        }

        public async Task<WinLossByMatchViewModel> Handle(WinLossByMatchQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await WinLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossByMatchViewModel(message, betlistData, Builder, TicketHelper);
        }
    }
}