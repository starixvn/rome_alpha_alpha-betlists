namespace Alpha.BetLists.Site.Handlers.WinLoss
{
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
        public WinLossHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(winLossService, builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
        }

        public async Task<WinLossViewModel> Handle(WinLossQuery message)
        {
            var winLossParam = CreateWinLossParam(message);
            var betlistData = await WinLossService.GetBetList(winLossParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new WinLossViewModel(message, betlistData, Builder, TicketHelper);
        }
    }
}