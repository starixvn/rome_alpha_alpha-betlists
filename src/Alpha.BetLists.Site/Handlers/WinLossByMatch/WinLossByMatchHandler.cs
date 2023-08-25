namespace Alpha.BetLists.Site.Handlers.WinLossByMatch
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.WinLossByMatch;
    using Services;
    using System.Threading.Tasks;
    using ViewModels.WinLossByMatch;

    public class WinLossByMatchHandler : BetListBaseHandler, IAsyncRequestHandler<WinLossByMatchQuery, WinLossByMatchViewModel>
    {
        private readonly WinLossByMatchService winLossByMatchService;

        public WinLossByMatchHandler(
           WinLossByMatchService winLossByMatchService,
           IBetListHTMLBuilder builder,
           ITicketHelper ticketHelper,
           ISmartPunterBetListMonitor monitorService,
           IPService ipService,
           IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.winLossByMatchService = winLossByMatchService;
        }

        public async Task<WinLossByMatchViewModel> Handle(WinLossByMatchQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await winLossByMatchService.GetBetList(winLossParam);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossByMatchViewModel(message, betlistData, Builder, TicketHelper);
        }

        protected internal static WinLossByMatchParam CreateWinLossParam(WinLossByMatchQuery message)
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