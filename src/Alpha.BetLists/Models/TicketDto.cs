namespace Alpha.BetLists.Models
{
    using Fanex.BetList.Core.Entities;

    public class TicketDto : Ticket
    {
        public string Country { get; set; }

        public bool IsInWatchlist { get; set; }

        public bool IsHighlight { get; set; }

        public string BetType { get; set; }
    }
}