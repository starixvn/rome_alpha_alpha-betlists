namespace Alpha.BetLists.Site.Handlers.FindTickets
{
    using System.Threading.Tasks;
    using Alpha.IPInfo.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.FindTickets;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.FindTickets;

    public class FindTicketsHandler : BetListBaseHandler, IAsyncRequestHandler<FindTicketsQuery, FindTicketsViewModel>
    {
        private readonly FindTicketsService FindTicketsService;

        public FindTicketsHandler(
           FindTicketsService FindTicketsService,
           IBetListHTMLBuilder builder,
           ITicketHelper ticketHelper,
           ISmartPunterBetListMonitor monitorService,
           IPService ipService,
           IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.FindTicketsService = FindTicketsService;
        }

        public async Task<FindTicketsViewModel> Handle(FindTicketsQuery message)
        {
            var param = new FindTicketsParam(message.RefNo, message.ViewerId, message.ViewerLevel);
            var betlistData = await FindTicketsService.GetBetList(param);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new FindTicketsViewModel(message, betlistData, Builder, TicketHelper);
        }
    }
}