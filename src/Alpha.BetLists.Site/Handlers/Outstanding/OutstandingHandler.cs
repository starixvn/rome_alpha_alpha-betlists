namespace Alpha.BetLists.Site.Handlers.Outstanding
{
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.Outstanding;
    using Services;
    using SmartPunter.Services;
    using System.Threading.Tasks;
    using Utilities.IP;
    using ViewModels.Outstanding;

    public class OutstandingHandler : BetListBaseHandler, IAsyncRequestHandler<OutstandingQuery, OutstandingViewModel>
    {
        private readonly OutstandingService outstandingService;

        public OutstandingHandler(
            OutstandingService oustandingService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.outstandingService = oustandingService;
        }

        public async Task<OutstandingViewModel> Handle(OutstandingQuery message)
        {
            var outstandingParam = new OutstandingParam(
                message.CustId,
                message.ProductIds.ToListInt(),
                message.FromDate,
                message.ToDate,
                message.GetAll,
                message.Language,
                message.CustName);

            var betlistData = await outstandingService.GetBetList(outstandingParam);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new OutstandingViewModel(message, betlistData, Builder, TicketHelper);
        }
    }
}