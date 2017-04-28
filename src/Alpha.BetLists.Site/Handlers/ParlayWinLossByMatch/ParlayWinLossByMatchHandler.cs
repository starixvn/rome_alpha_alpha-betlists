using System.Threading.Tasks;
using Alpha.BetLists.Models;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.BetLists.Site.ViewModels.ParlayWinLossByMatch;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch
{
    public class ParlayWinLossByMatchHandler
        : ParlayWinLossByMatchBaseHandler, IAsyncRequestHandler<ParlayWinLossByMatchQuery, ParlayWinLossByMatchViewModel>
    {
        public ParlayWinLossByMatchHandler(
            ParlayWinLossByMatchService winLossService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService)
            : base(winLossService, monitorService, builder, excelBuilder, ticketHelper, ipService)
        {
        }

        public async Task<ParlayWinLossByMatchViewModel> Handle(ParlayWinLossByMatchQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var tickets = await WinLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new ParlayWinLossByMatchViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
        }
    }
}