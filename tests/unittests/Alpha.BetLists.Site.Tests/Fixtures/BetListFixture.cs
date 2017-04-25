namespace Alpha.BetLists.Site.Tests.Fixtures
{
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using SmartPunter.Services;
    using Utilities.IP;

    public class BetListFixture
    {
        public BetListFixture()
        {
            HtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            ExcelBuilder = Substitute.For<IBetListExcelBuilder>();
            TicketHelper = Substitute.For<ITicketHelper>();
            MoniterService = Substitute.For<ISmartPunterBetListMonitor>();
            IpService = Substitute.For<IPService>();
        }

        public IBetListHTMLBuilder HtmlBuilder { get; }

        public IBetListExcelBuilder ExcelBuilder { get; }

        public ITicketHelper TicketHelper { get; }

        public ISmartPunterBetListMonitor MoniterService { get; }

        public IPService IpService { get; }
    }
}