using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch
{
    public class ParlayWinLossByMatchBaseHandler : BetListBaseHandler
    {
        protected internal readonly ParlayWinLossByMatchService WinLossService;

        public ParlayWinLossByMatchBaseHandler(
            ParlayWinLossByMatchService winLossService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            WinLossService = winLossService;
        }

        protected internal ParlayWinLossByMatchParam CreateWinLossParam(ParlayWinLossByMatchQuery message)
            => new ParlayWinLossByMatchParam(
                message.CustId,
                message.BetType,
                message.Language,
                message.FromDate,
                message.ToDate,
                message.CustLevel);
    }
}