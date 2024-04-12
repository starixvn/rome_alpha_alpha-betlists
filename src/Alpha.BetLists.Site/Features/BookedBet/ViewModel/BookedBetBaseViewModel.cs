namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Helpers;
    using Alpha.BetLists.Site.ViewModels;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using System.Collections.Generic;

    public class BookedBetBaseViewModel : BetListBaseViewModel
    {
        public BookedBetBaseViewModel(
            BookedBetBaseQuery query,
            IEnumerable<TicketDto> tickets,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper) : base(query, new BetListData(tickets, null), builder, ticketHelper, true)
        {
            const int totalColumn = 7;
            TotalColumn = totalColumn;
        }

        public string GetReason(TicketDto ticket)
            => ticket.StatusId == BookedBetStatusConstants.Unsuccessful ? CacheRefServiceHelper.GetBookedBetReason(ticket.Reason) : string.Empty;
    }
}