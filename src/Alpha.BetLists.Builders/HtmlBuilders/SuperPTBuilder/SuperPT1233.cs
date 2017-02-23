namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.BetList.Core.Entities;

    public class SuperPT1233 : SuperPT33
    {
        public SuperPT1233()
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