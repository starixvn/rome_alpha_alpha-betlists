namespace Alpha.BetLists.Site.Handlers
{
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using Requests.Parlay;
    using Services;
    using ViewModels.Parlay;

    public class ParlayHandler :
        IAsyncRequestHandler<MixParlayQuery, MixParlayViewModel>,
        IAsyncRequestHandler<CombinationMixParlayQuery, CombinationMixParlayViewModel>
    {
        private readonly ParlayService parlayService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;

        public ParlayHandler(
            ParlayService mixParlayService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            this.parlayService = mixParlayService;
            this.builder = builder;
            this.ticketHelper = ticketHelper;
        }

        public async Task<MixParlayViewModel> Handle(MixParlayQuery message)
        {
            var tickets = await parlayService.GetMixParlayDetail(message.TransId, message.Language);

            return new MixParlayViewModel(message, new BetListData(tickets, null), builder, ticketHelper);
        }

        public async Task<CombinationMixParlayViewModel> Handle(CombinationMixParlayQuery message)
        {
            var tickets = await parlayService.GetCombinationMixParlayDetail(message.TransId, message.Language);

            return new CombinationMixParlayViewModel(message, new BetListData(tickets, null), builder, ticketHelper);
        }
    }
}