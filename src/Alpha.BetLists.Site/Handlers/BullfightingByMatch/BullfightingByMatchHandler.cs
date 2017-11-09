namespace Alpha.BetLists.Site.Handlers.RacingByMatch
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

    public class BullfightingByMatchHandler : BetListBaseHandler, IAsyncRequestHandler<BullfightingByMatchQuery, BullfightingByMatchViewModel>
    {        
        private readonly BullfightingByMatchService service;

        public BullfightingByMatchHandler(
            BullfightingByMatchService service,            
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {            
            this.service = service;
        }

        public async Task<BullfightingByMatchViewModel> Handle(BullfightingByMatchQuery message)
        {
            var param = CreateRacingByMatchParam(message);

            var tickets = await service.GetBetList(param);            

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, tickets);

            return new BullfightingByMatchViewModel(
                message,
                new BetListData(tickets, null),
                Builder,
                TicketHelper);
        }

        protected internal static BullfightingByMatchParam CreateRacingByMatchParam(BullfightingByMatchQuery message)
            => new BullfightingByMatchParam(
                message.ViewerId,                
                message.MatchId,
                message.ViewerLevel);
    }
}
