using Alpha.BetLists.Params;
using Alpha.BetLists.Site.Requests.WinLossByMatch;

namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    public class WinLossByMatchBaseHandler : BetListBaseHandler
    {
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