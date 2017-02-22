namespace Alpha.BetLists.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;

    public class BetListData
    {
        public BetListData(
            IEnumerable<ITicket> tickets,
            IEnumerable<ITicketData> ticketData)
        {
            Tickets = tickets ?? Enumerable.Empty<ITicket>();
            TicketData = ticketData ?? Enumerable.Empty<ITicketData>();
        }

        public IEnumerable<ITicket> Tickets { get; }

        public IEnumerable<ITicketData> TicketData { get; }
    }
}