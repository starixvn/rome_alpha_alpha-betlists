namespace Alpha.BetLists.Site.Handlers.ParlayWinLossByMatch
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.WinLossByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.ParlayWinLossByMatch;

    public class ParlayWinLossByMatchHandler
        : BetListBaseHandler, IAsyncRequestHandler<ParlayWinLossByMatchQuery, ParlayWinLossByMatchViewModel>
    {
        private readonly ParlayWinLossByMatchService parlayWinLossByMatchService;

        public ParlayWinLossByMatchHandler(
            ParlayWinLossByMatchService parlayWinLossByMatchService,
            ISmartPunterBetListMonitor monitorService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.parlayWinLossByMatchService = parlayWinLossByMatchService;
        }

        public async Task<ParlayWinLossByMatchViewModel> Handle(ParlayWinLossByMatchQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var tickets = await parlayWinLossByMatchService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new ParlayWinLossByMatchViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
        }

        protected internal static ParlayWinLossByMatchParam CreateWinLossParam(ParlayWinLossByMatchQuery message)
          => new ParlayWinLossByMatchParam(
              message.CustId,
              message.BetType,
              message.Language,              
              message.FromDate,
              message.ToDate,
              message.LimitedDateToShowInfo,
              message.CustLevel);
    }
}