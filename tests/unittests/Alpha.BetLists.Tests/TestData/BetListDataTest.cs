namespace Alpha.BetLists.Tests.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetListNPOI.HSSF.UserModel;
    using BetListNPOI.SS.UserModel;
    using BetLists.Models;
    using BetLists.Models.Parlay;
    using Fanex.BetList.Core.Entities;
    using Ploeh.AutoFixture;

    public class BetListDataTest
    {
        private static IFixture fixture = new Fixture();

        public static readonly CashOutHistoryModel CashOutHistory = fixture.Create<CashOutHistoryModel>();

        public static readonly IEnumerable<TicketDto> Tickets = fixture.CreateMany<TicketDto>();

        public static readonly IEnumerable<TicketDto> OldTickets = fixture.CreateMany<TicketDto>();

        public static readonly IEnumerable<TicketData> TicketData = fixture.CreateMany<TicketData>();

        public static readonly IEnumerable<TicketData> OldTicketData = fixture.CreateMany<TicketData>();

        public static readonly IEnumerable<int> ProductIds = fixture.CreateMany<int>();

        public static readonly IEnumerable<Product> Products = fixture.CreateMany<Product>();

        public const string ProductDetailsXML
            = "<ROOT><ProductDetail ProductId = \"42\" Sporttype = \"200\" Bettype = \"0\"/><ProductDetail ProductId = \"42\" Sporttype = \"150\" Bettype = \"0\"/></ROOT>";

        public static readonly IEnumerable<int> BettypeIds = fixture.CreateMany<int>();

        public static readonly (IEnumerable<TicketDto>, IEnumerable<TicketData>) BetListDataTuple = (Tickets, TicketData);

        public static readonly (IEnumerable<TicketDto>, IEnumerable<TicketData>) OldBetListDataTuple = (OldTickets, OldTicketData);

        public static readonly BetListData BetListData
            = new BetListData(Tickets, TicketData.ToList<ITicketData>());

        public static readonly BetListData OldBetListData
            = new BetListData(OldTickets, OldTicketData.ToList<ITicketData>());

        public static readonly IEnumerable<ComboParlayDetail> ComboParlayDetails = fixture.CreateMany<ComboParlayDetail>();

        public static readonly IEnumerable<ComboParlayDetail> ComboParlayDetailsHuge = fixture.CreateMany<ComboParlayDetail>(5001);

        public static readonly IEnumerable<MatchDetail> MatchDetails = fixture.CreateMany<MatchDetail>();

        public static readonly (IEnumerable<ComboParlayDetail>, IEnumerable<MatchDetail>) ComboParlayDetailTuple = (ComboParlayDetails, MatchDetails);

        public static readonly ComboParlayDetailData ComboParlayDetailData
            = new ComboParlayDetailData(ComboParlayDetails, MatchDetails, 0, false);

        public static readonly IRichTextString TransRtf = new HSSFRichTextString("Trans");

        public static readonly List<IRichTextString> ChoiceRtf = new List<IRichTextString> { new HSSFRichTextString("Choice") };

        public static readonly IRichTextString OddsRtf = new HSSFRichTextString("Odds");

        public static readonly IRichTextString StakeRtf = new HSSFRichTextString("Stake");

        public static readonly IRichTextString StatusRtf = new HSSFRichTextString("Status");

        public static readonly byte[] ExcelFileContent = fixture.CreateMany<byte>().ToArray();

        public static TicketDto Ticket => fixture.Create<TicketDto>();
    }
}