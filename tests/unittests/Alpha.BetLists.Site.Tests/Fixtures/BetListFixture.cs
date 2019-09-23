namespace Alpha.BetLists.Site.Tests.Fixtures
{
    using Alpha.BetLists.Site.Helpers;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using NPOI.HSSF.UserModel;
    using NSubstitute;
    using SmartPunter.Services;
    using Utilities.IP;

    public class BetListFixture
    {
        public BetListFixture()
        {
            BrowserHelper = Substitute.For<BrowserHelper>();
            HtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            ExcelBuilder = Substitute.For<IBetListExcelBuilder>();
            TicketHelper = Substitute.For<ITicketHelper>();
            MonitorService = Substitute.For<ISmartPunterBetListMonitor>();
            IpService = Substitute.For<IPService>();
            ExcelBuilder.RTFHelper.Returns(CreateRtfHelper());
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

        public RTFHelper RtfHelper { get; internal set; }

        public ISmartPunterBetListMonitor MonitorService { get; }

        public BrowserHelper BrowserHelper { get; }

        public IPService IpService { get; }

        public IUser User { get; }

        private RTFHelper CreateRtfHelper()
        {
            var workbook = new HSSFWorkbook();

            return new RTFHelper()
            {
                NegFont = workbook.CreateFont(),
                NormalFont = workbook.CreateFont(),
                PosFont = workbook.CreateFont(),
                NegFontCrossed = workbook.CreateFont(),
                NormalFontCrossed = workbook.CreateFont(),
                PosFontCrossed = workbook.CreateFont(),
                RTFRenderer = new RtfTextRender()
            };
        }
    }
}