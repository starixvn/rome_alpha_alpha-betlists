namespace Alpha.BetLists.Tests.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Models;
    using Ploeh.AutoFixture;

    public class BetListDataTest
    {
        private static IFixture fixture = new Fixture();

        public static readonly IEnumerable<Ticket> Tickets = fixture.CreateMany<Ticket>();

        public static readonly IEnumerable<Ticket> OldTickets = fixture.CreateMany<Ticket>();

        public static readonly IEnumerable<TicketData> TicketData = fixture.CreateMany<TicketData>();

        public static readonly IEnumerable<TicketData> OldTicketData = fixture.CreateMany<TicketData>();

        public static readonly IEnumerable<int> ProductIds = fixture.CreateMany<int>();

        public static readonly Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> BetListDataTuple
            = new Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>>(Tickets, TicketData);

        public static readonly Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>> OldBetListDataTuple
            = new Tuple<IEnumerable<Ticket>, IEnumerable<TicketData>>(OldTickets, OldTicketData);

        public static readonly BetListData BetListData
            = new BetListData(Tickets, TicketData.ToList<ITicketData>());

        public static readonly BetListData OldBetListData
            = new BetListData(OldTickets, OldTicketData.ToList<ITicketData>());
    }
}