namespace Alpha.BetLists.Site.Tests.Handler.MemberBetList
{
    using Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using AlphaPlus.Utilities.IP;
    using BetLists.Tests.TestData;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Services;
    using System;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("BetListCollection")]
    public class RiskAnalysisIPMatchingHelperTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly RiskAnalysisIPMatchingHelperImpl riskAnalysisIPMatchingHelperImpl;
        private readonly RiskAnalysisIPMatchingService service;
        private readonly ReportDateService reportDateService;
        private readonly IPService ipService;

        public RiskAnalysisIPMatchingHelperTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            service = Substitute.For<RiskAnalysisIPMatchingService>();
            ipService = Substitute.For<IPService>();
            reportDateService = Substitute.For<ReportDateService>();

            service.GetBetList(Arg.Any<RiskAnalysisIPMatchingParam>()).Returns(BetListDataTest.BetListData);

            riskAnalysisIPMatchingHelperImpl = new RiskAnalysisIPMatchingHelperImpl( service, reportDateService, ipService);
        }

        [Fact]
        public void BuildRiskAnalysisIPMatchingViewModel_ValidMessage_ReturnCorrectViewModel()
        {
            // Arrange
            betlistFixture.User.IsAdminCanViewVNIP.Returns(true);
            betlistFixture.User.LevelId.Returns((byte)4);
            ipService.IsVNIP(Arg.Any<string>()).Returns(true);

            var message = new RiskAnalysisIPMatchingQuery(betlistFixture.User)
            {
                CustId = 1,
                Date = DateTime.Today,
                CustName = "192.168.1.1"
            };

            // Act
            var actualMemberBetListViewModel = riskAnalysisIPMatchingHelperImpl.BuildRiskAnalysisIPMatchingViewModel(message).GetAwaiter().GetResult();

            // Assert
            BetListDataTest.BetListData.Equals(actualMemberBetListViewModel);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(2)]
        public void BuildRiskAnalysisIPMatchingViewModel_NotSuperLevel_ReturnNull(byte levelId)
        {
            // Arrange
            betlistFixture.User.IsAdminCanViewVNIP.Returns(true);
            betlistFixture.User.LevelId.Returns(levelId);
            ipService.IsVNIP(Arg.Any<string>()).Returns(true);

            var message = new RiskAnalysisIPMatchingQuery(betlistFixture.User)
            {
                CustId = 1,
                Date = DateTime.Today,
                CustName = "192.168.1.1"
            };

            // Act
            var actualMemberBetListViewModel = riskAnalysisIPMatchingHelperImpl.BuildRiskAnalysisIPMatchingViewModel(message).GetAwaiter().GetResult();

            // Assert
            Assert.Null(actualMemberBetListViewModel);
        }

        [Fact]
        public void BuildRiskAnalysisIPMatchingViewModel_NotAllowDisplayVNIP_And_IPIsVNIP_ReturnNull()
        {
            // Arrange
            betlistFixture.User.IsAdminCanViewVNIP.Returns(false);
            betlistFixture.User.LevelId.Returns((byte)4);
            ipService.IsVNIP(Arg.Any<string>()).Returns(true);

            var message = new RiskAnalysisIPMatchingQuery(betlistFixture.User)
            {
                CustId = 1,
                Date = DateTime.Today,
                CustName = "192.168.1.1"
            };

            // Act
            var actualMemberBetListViewModel = riskAnalysisIPMatchingHelperImpl.BuildRiskAnalysisIPMatchingViewModel(message).GetAwaiter().GetResult();

            // Assert
            Assert.Null(actualMemberBetListViewModel);
        }
    }
}