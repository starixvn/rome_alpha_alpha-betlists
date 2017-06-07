using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Requests.CancelledBet;
using Alpha.BetLists.Site.ViewModels.CancelledBet;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using System.Threading.Tasks;

namespace Alpha.BetLists.Site.Handlers.CancelledBet
{
    public class CancelledBetHandler : IAsyncRequestHandler<CancelledBetQuery, CancelledBetViewModel>
    {
        private readonly CancelledBetService Service;
        private readonly IBetListHTMLBuilder Builder;
        private readonly ITicketHelper TicketHelper;

        public CancelledBetHandler(
            CancelledBetService service,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            Service = service;
            Builder = builder;
            TicketHelper = ticketHelper;
        }

        public async Task<CancelledBetViewModel> Handle(CancelledBetQuery message)
        {
            var parameter = new CancelledBetParam(message.ViewerLevel, message.ViewerId, message.MasterId, message.AgentId, message.MemberId, message.Language);
            var cancelledBet = await Service.GetCancelledBet(parameter);

            return new CancelledBetViewModel(message, cancelledBet, Builder, TicketHelper);
        }
    }
}