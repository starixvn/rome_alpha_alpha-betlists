namespace Alpha.BetLists.Site.Tests.Handler.MemberBetList
{
    using Alpha.BetLists.Site.Handlers.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.Requests.RiskAnalysisIPMatching;
    using Alpha.BetLists.Site.ViewModels.RiskAnalysisIPMatching;
    using BetLists.Tests.TestData;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Services;
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    [Collection("BetListCollection")]
    public class RiskAnalysisIPMatchingHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly RiskAnalysisIPMatchingHandler handler;
        private readonly RiskAnalysisIPMatchingService service;
        private readonly ReportDateService reportDateService;
        private readonly RiskAnalysisIPMatchingHelper riskAnalysisIPMatchingHelper;
        private readonly CompareLogic compareLogic;

        public RiskAnalysisIPMatchingHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            service = Substitute.For<RiskAnalysisIPMatchingService>();
            reportDateService = Substitute.For<ReportDateService>();

            riskAnalysisIPMatchingHelper = Substitute.For<RiskAnalysisIPMatchingHelper>();
            riskAnalysisIPMatchingHelper
                .BuildRiskAnalysisIPMatchingViewModel(Arg.Any<RiskAnalysisIPMatchingQuery>())
                .Returns(BetListDataTest.BetListData);

            handler = new RiskAnalysisIPMatchingHandler(
                service,
                reportDateService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
                this.betlistFixture.IpService,
                riskAnalysisIPMatchingHelper);

            compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "ResultsPath", "OldMainPath" }
            });
        }

        [Fact]
        public void Handle_AnyProductIds_ReturnCorrectViewModel()
        {
            // Arrange
            var message = new RiskAnalysisIPMatchingQuery(betlistFixture.User);
            message.CustId = 1;
            message.Date = DateTime.Today;
            message.CustName = "192.168.1.1";

            var expectedMemberBetListViewModel = new RiskAnalysisIPMatchingViewModel(
                message,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualMemberBetListViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualMemberBetListViewModel, expectedMemberBetListViewModel).AreEqual);
        }
    }
}