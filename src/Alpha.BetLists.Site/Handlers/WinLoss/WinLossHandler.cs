namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using System.Threading.Tasks;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.WinLoss;

    public class WinLossHandler : BetListBaseHandler, IAsyncRequestHandler<WinLossQuery, WinLossViewModel>
    {
        private readonly WinLossService winLossService;

        public WinLossHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            this.winLossService = winLossService;
        }

        public async Task<WinLossViewModel> Handle(WinLossQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await winLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossViewModel(message, betlistData, Builder, TicketHelper);
        }

        protected internal static WinLossParam CreateWinLossParam(WinLossQuery message)
            => new WinLossParam(
               message.CustId,
               message.ProductIds.ToListInt(),
               message.FromDate,
               message.ToDate,
               message.Language);
    }
}