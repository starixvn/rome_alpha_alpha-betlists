namespace Alpha.BetLists.Site.Features.BookedBet
{
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using System.Collections.Generic;

    public class BookedBetDataViewModel : BookedBetBaseViewModel
    {
        public BookedBetDataViewModel(
            BookedBetDataQuery query,
            IEnumerable<TicketDto> tickets,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper) : base(query, tickets, builder, ticketHelper)
        {
        }
    }
}