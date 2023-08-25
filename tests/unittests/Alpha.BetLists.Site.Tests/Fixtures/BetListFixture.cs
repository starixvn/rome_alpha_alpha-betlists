namespace Alpha.BetLists.Site.Tests.Fixtures
{
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Helpers;
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using BetListNPOI.HSSF.UserModel;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
            IpInfoService = Substitute.For<IIPInfoService>();
            IpInfoService
                .GetCountries(Arg.Any<IEnumerable<string>>())
                .Returns(Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string>()));
            AutoFollowBetService = Substitute.For<AutoFollowBetService>();
            ExcelBuilder.RTFHelper.Returns(CreateRtfHelper());
            RtfHelper = ExcelBuilder.RTFHelper;

            User = Substitute.For<IUser>();
            User.LevelId.Returns((byte)4);
            User.Id.Returns(10);
            User.Name.Returns("Name");
            User.Language.Returns("en-US");

            LastBetsService = Substitute.For<LastBetsService>();
        }

        public IBetListHTMLBuilder HtmlBuilder { get; }

        public IBetListExcelBuilder ExcelBuilder { get; }

        public ITicketHelper TicketHelper { get; }

        public RTFHelper RtfHelper { get; internal set; }

        public ISmartPunterBetListMonitor MonitorService { get; }

        public BrowserHelper BrowserHelper { get; }

        public IPService IpService { get; }

        public IIPInfoService IpInfoService { get; }

        public AutoFollowBetService AutoFollowBetService { get; }

        public IUser User { get; }

        public LastBetsService LastBetsService { get; }

        private RTFHelper CreateRtfHelper()
        {
            var workbook = new HSSFWorkbook();

            return new RTFHelper
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