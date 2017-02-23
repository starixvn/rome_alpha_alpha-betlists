namespace Alpha.BetLists.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;

    public class BetListData
    {
        public BetListData(
            IEnumerable<ITicket> tickets,
            List<ITicketData> ticketData)
        {
            Tickets = tickets ?? Enumerable.Empty<ITicket>();
            TicketData = ticketData ?? new List<ITicketData>();
            Total = Tickets.Count();
        }

        public IEnumerable<ITicket> Tickets { get; }

        public List<ITicketData> TicketData { get; }

        public int Total { get; }
    }
}