namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Params;
    using Requests.WinLoss;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;

    public class WinLossBaseHandler : BetListBaseHandler
    {
        public WinLossBaseHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            WinLossService = winLossService;
        }

        protected WinLossService WinLossService { get; }

        protected WinLossParam CreateWinLossParam(WinLossQuery message)
        {
            return new WinLossParam(
               message.CustId,
               message.ProductIds.ToListInt(),
               message.FromDate,
               message.ToDate,
               message.Language);
        }
    }
}