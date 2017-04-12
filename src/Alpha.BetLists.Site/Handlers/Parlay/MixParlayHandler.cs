namespace Alpha.BetLists.Site.Handlers.Parlay
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.Parlay;
    using Services;
    using ViewModels.Parlay;

    public class MixParlayHandler : IAsyncRequestHandler<MixParlayQuery, MixParlayViewModel>
    {
        private readonly MixParlayService mixParlayService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;

        public MixParlayHandler(
            MixParlayService mixParlayService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            this.mixParlayService = mixParlayService;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
        }

        public async Task<MixParlayViewModel> Handle(MixParlayQuery message)
        {
            var tickets = await mixParlayService.GetParlayDetail(message.TransId, message.Language);

            return new MixParlayViewModel(message, new BetListData(tickets, null), builder, ticketHelper);
        }
    }
}