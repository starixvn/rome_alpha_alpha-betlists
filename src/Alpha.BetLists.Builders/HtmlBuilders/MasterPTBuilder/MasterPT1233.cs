namespace Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    public class MasterPT1233 : MasterPT33
    {
        public MasterPT1233()
        {
            WinBetId = 1231;
            PlaceBetId = 1232;
        }

        protected override List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.RefNo));
        }
    }
}