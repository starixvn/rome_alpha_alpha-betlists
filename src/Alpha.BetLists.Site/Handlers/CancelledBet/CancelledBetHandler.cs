namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    using System.Threading.Tasks;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;

    public class CancelledBetHandler : IAsyncRequestHandler<CancelledBetQuery, CancelledBetViewModel>
    {
        private readonly CancelledBetService service;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;

        public CancelledBetHandler(
            CancelledBetService service,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            IPService ipService)
        {
            this.service = service;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
            this.IpService = ipService;
            this.ticketHelper.IsVnIp = ipService.IsVNIP;
        }

        protected internal IPService IpService { get; }

        public async Task<CancelledBetViewModel> Handle(CancelledBetQuery message)
        {
            var parameter = new CancelledBetParam(message.ViewerLevel, message.ViewerId, message.MasterId, message.AgentId, message.MemberId, message.Language);
            var cancelledBet = await service.GetCancelledBet(parameter);

            return new CancelledBetViewModel(message, cancelledBet, builder, ticketHelper);
        }
    }
}