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

    public class ParlayHandler : ParlayBaseHandler,
        IAsyncRequestHandler<MixParlayQuery, MixParlayViewModel>,
        IAsyncRequestHandler<ComboParlayQuery, ComboParlayViewModel>,
        IAsyncRequestHandler<ComboParlayDetailQuery, ComboParlayDetailViewModel>
    {
        private readonly IBetListHTMLBuilder builder;

        public ParlayHandler(
            ParlayService parlayService,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper) : base(parlayService, ticketHelper)
        {
            this.builder = builder;
        }

        public async Task<MixParlayViewModel> Handle(MixParlayQuery message)
        {
            var tickets = await ParlayService.GetParlayBetList(message.TransId, message.Language);

            return new MixParlayViewModel(message, new BetListData(tickets, null), builder, TicketHelper);
        }

        public async Task<ComboParlayViewModel> Handle(ComboParlayQuery message)
        {
            var tickets = await ParlayService.GetParlayBetList(message.TransId, message.Language);

            return new ComboParlayViewModel(message, new BetListData(tickets, null), builder, TicketHelper);
        }

        public async Task<ComboParlayDetailViewModel> Handle(ComboParlayDetailQuery message)
            => await GetComboParlayDetail(message);
    }
}