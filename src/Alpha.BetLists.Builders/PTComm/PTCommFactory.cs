namespace Alpha.BetLists.Builders.PTComm
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Models;

    public static class PTCommFactory
    {
        public static PTComm CreateInstance(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
        {
            if (ticket.BetTypeId == 33 || ticket.BetTypeId == 1233)
            {
                return new PTComm33(ticket, ticketData, userLevel).InitPtCommData();
            }

            if (ticket.BetTypeId == 500)
            {
                return new PTComm500(ticket, userLevel).InitPtCommData();
            }

            if (ticket.BetTypeId == 501)
            {
                return new PTComm501(ticket, userLevel).InitPtCommData();
            }

            if (ticket.BetTypeId == 71 || ticket.BetTypeId == 73)
            {
                return new PTComm71(ticket, userLevel).InitPtCommData();
            }

            return new PTComm(ticket, userLevel).InitPtCommData();
        }
    }
}