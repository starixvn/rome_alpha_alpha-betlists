namespace Alpha.BetLists.Site.Handlers.SearchTickets
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.SearchTickets;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using ViewModels.SearchTickets;

    public class SearchTicketsHandler : BetListBaseHandler, IAsyncRequestHandler<SearchTicketsQuery, SearchTicketsViewModel>
    {
        private readonly SearchTicketsService searchTicketsService;

        public SearchTicketsHandler(
           SearchTicketsService SearchTicketsService,
           IBetListHTMLBuilder builder,
           ITicketHelper ticketHelper,
           ISmartPunterBetListMonitor monitorService,
           IPService ipService,
           IIPInfoService ipInfoService) : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.searchTicketsService = SearchTicketsService;
        }

        public async Task<SearchTicketsViewModel> Handle(SearchTicketsQuery message)
        {
            var param = new SearchTicketsParam(message.RefNo, message.ViewerId, message.ViewerLevel);
            var betlistData = await searchTicketsService.GetBetList(param);
            if (betlistData.Tickets.Any())
            {
                await AssignCountry(betlistData.Tickets, message.DisplayVNIP);
                WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);
            }
            return new SearchTicketsViewModel(message, betlistData, Builder, TicketHelper);
        }
    }
}