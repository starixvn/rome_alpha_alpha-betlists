namespace Alpha.BetLists.Site.Tests.Fixtures
{
    using BetLists.Models;
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
            RtfHelper = ExcelBuilder.RTFHelper;

            User = Substitute.For<IUser>();
            User.LevelId.Returns((byte)4);
            User.Id.Returns(10);
            User.Name.Returns("Name");
            User.Language.Returns("en-US");
        }

        public IBetListHTMLBuilder HtmlBuilder { get; }

        public IBetListExcelBuilder ExcelBuilder { get; }

        public ITicketHelper TicketHelper { get; }

        public RTFHelper RtfHelper { get; }

        public ISmartPunterBetListMonitor MoniterService { get; }

        public IPService IpService { get; }

        public IUser User { get; }
    }
}