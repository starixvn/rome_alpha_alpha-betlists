using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.WinLossByMatch;
using Alpha.SmartPunter.Services;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    public class WinLossByMatchBaseHandler : BetListBaseHandler
    {
        protected readonly WinLossByMatchService WinLossService;

        public WinLossByMatchBaseHandler(
            WinLossByMatchService winLossService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            WinLossService = winLossService;
        }

        protected WinLossByMatchParam CreateWinLossParam(WinLossByMatchQuery message)
            => new WinLossByMatchParam(
                message.CustId,
                message.LeagueId,
                message.MatchId,
                message.BetType,
                message.Race,
                message.LiveIndicator,
                message.Language,
                message.CustName,
                message.FromDate,
                message.ToDate,
                message.CustLevel);
    }
}