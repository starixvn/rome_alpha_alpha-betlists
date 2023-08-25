namespace Alpha.BetLists.Site.Handlers.MoveStatementHistory
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.MoveStatementHistory;
    using Alpha.BetLists.Site.ViewModels.MoveStatementHistory;
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using System.Threading.Tasks;

    public class MoveStatementHistoryHandler : BetListBaseHandler, IAsyncRequestHandler<MoveStatementHistoryQuery, MoveStatementHistoryViewModel>
    {
        private readonly MoveStatementHistoryService moveStatementHistoryService;

        public MoveStatementHistoryHandler(
            MoveStatementHistoryService moveStatementHistoryService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.moveStatementHistoryService = moveStatementHistoryService;
        }

        public async Task<MoveStatementHistoryViewModel> Handle(MoveStatementHistoryQuery message)
        {
            var moveStatementHistoryParam = new MoveStatementHistoryParam(message.CustId, message.TransId);
            var tickets = await moveStatementHistoryService.GetBetList(moveStatementHistoryParam);
            await AssignCountry(tickets, message.DisplayVNIP);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new MoveStatementHistoryViewModel(message, new BetListData(tickets, null), Builder, TicketHelper);
        }
    }
}