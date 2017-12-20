namespace Alpha.BetLists.Site.Handlers.BullfightingByMatch
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Params;
    using Requests.BullfightingByMatch;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels.BullfightingByMatch;

    public class BullfightingByMatchHandler : IAsyncRequestHandler<BullfightingByMatchQuery, BullfightingByMatchViewModel>
    {        
        private readonly BullfightingByMatchService service;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;        

        public BullfightingByMatchHandler(
            BullfightingByMatchService service,            
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IPService ipService)
        {
            this.service = service;
            this.builder = builder;
            this.ticketHelper = ticketHelper;            
            this.ticketHelper.IsVnIp = ipService.IsVNIP;
        }       

        public async Task<BullfightingByMatchViewModel> Handle(BullfightingByMatchQuery message)
        {
            var param = CreateParam(message);
            var tickets = await service.GetBetList(param);

            return new BullfightingByMatchViewModel(
                message,
                new BetListData(tickets, null),
                builder,
                ticketHelper);
        }

        protected internal static BullfightingByMatchParam CreateParam(BullfightingByMatchQuery message)
            => new BullfightingByMatchParam(                
                message.ViewerId,                
                message.MatchId,
                message.ViewerLevel);
    }
}
