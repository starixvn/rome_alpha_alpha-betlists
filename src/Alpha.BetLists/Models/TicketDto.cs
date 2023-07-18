namespace Alpha.BetLists.Models
{
    using Alpha.BetLists.Helpers;
    using Fanex.BetList.Core.Entities;

    public class TicketDto : Ticket
    {
        public string Country { get; set; }

        public string BetType => BetTypeHelper.GetBetTypeName(BetTypeId);
    }
}