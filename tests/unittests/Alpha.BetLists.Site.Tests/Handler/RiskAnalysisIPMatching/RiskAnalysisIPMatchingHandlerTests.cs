namespace Alpha.BetLists.Site.Tests.Handler.MemberBetList
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using BetLists.Tests.TestData;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Services;
    using System;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("BetListCollection")]
    public class RiskAnalysisIPMatchingHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly RiskAnalysisIPMatchingHandler riskAnalysisIPMatchingHandler;
        private readonly RiskAnalysisIPMatchingHelper riskAnalysisIPMatchingHelper;
        private readonly RiskAnalysisIPMatchingService service;
        private readonly ReportDateService reportDateService;

        public RiskAnalysisIPMatchingHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            service = Substitute.For<RiskAnalysisIPMatchingService>();
            riskAnalysisIPMatchingHelper = Substitute.For<RiskAnalysisIPMatchingHelper>();
            reportDateService = Substitute.For<ReportDateService>();

            riskAnalysisIPMatchingHandler = new RiskAnalysisIPMatchingHandler(
                service, 
                reportDateService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
                this.betlistFixture.IpService,
                riskAnalysisIPMatchingHelper,
                this.betlistFixture.IpInfoService);
        }

        [Theory]
        [InlineData(null)]
        public void Handle_NotAllowDisplayVNIP_BetlistDataIsNull_ReturnNull(BetListData betData)
        {
            // Arrange
            riskAnalysisIPMatchingHelper.BuildRiskAnalysisIPMatchingViewModel(Arg.Any<RiskAnalysisIPMatchingQuery>()).ReturnsForAnyArgs(betData);

            var message = new RiskAnalysisIPMatchingQuery(betlistFixture.User)
            {
                CustId = 1,
                Date = DateTime.Today,
                CustName = "192.168.1.1"
            };

            // Act
            var actualResult = riskAnalysisIPMatchingHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.Null(actualResult);
        }


        [Fact]
        public void Handle_NotAllowDisplayVNIP_BetlistDataIsNotNull_ReturnCorrectData()
        {
            // Arrange
            riskAnalysisIPMatchingHelper
                .BuildRiskAnalysisIPMatchingViewModel(Arg.Any<RiskAnalysisIPMatchingQuery>())
                .ReturnsForAnyArgs(BetListDataTest.BetListData);

            var message = new RiskAnalysisIPMatchingQuery(betlistFixture.User)
            {
                CustId = 1,
                Date = DateTime.Today,
                CustName = "192.168.1.1"
            };

            // Act
            var actualResult = riskAnalysisIPMatchingHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            actualResult.Equals(BetListDataTest.BetListData);
        }
    }
}