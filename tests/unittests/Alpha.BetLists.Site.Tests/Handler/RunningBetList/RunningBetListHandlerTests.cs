namespace Alpha.BetLists.Site.Tests.Handler.RunningBetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Site.ViewModels.Components;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.RunningBetList;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.RunningBetList;
    using Services;
    using Site.ViewModels.RunningBetList;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    [Collection("BetListCollection")]
    public class RunningBetListHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly RunningBetListHandler runningBetListHandler;
        private readonly RunningBetListService runningBetListService;
        private readonly ProductService productService;
        private readonly ForecastService forecastService;
        private readonly RunningBetListQuery query;
        private readonly CompareLogic compareLogic;

        public RunningBetListHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            runningBetListService = Substitute.For<RunningBetListService>();
            runningBetListService.GetBetList(Arg.Any<RunningBetListParam>()).Returns(BetListDataTest.BetListData);

            productService = Substitute.For<ProductService>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<IEnumerable<int>>(), false).Returns(BetListDataTest.ProductIds);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            query = new RunningBetListQuery(this.betlistFixture.User);

            runningBetListHandler = new RunningBetListHandler(
                runningBetListService,
                productService,
                forecastService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
                this.betlistFixture.IpService);

            compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "ResultsPath", "OldMainPath" }
            });
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        public void Handle_Always_CallWriteLog(int custId, int expectedId)
        {
            // Arrange
            query.CustId = custId;

            // Act
            runningBetListHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MonitorService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_AnyProductIds_ReturnCorrectViewModel()
        {
            // Arrange
            query.CustId = 1;
            query.ProductIds = "1";
            query.GetAll = true;
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.LimitedDateToShowInfo = DateTime.MinValue;
            query.Language = "en";
            query.CustName = "custName";

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<IEnumerable<int>>(), false).Returns(validUserSelectedProductIds);

            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            var runningBetListParam = new RunningBetListParam(
                1,
                validUserSelectedProductIds,
                query.FromDate,
                query.ToDate,
                query.LimitedDateToShowInfo,
                query.GetAll,
                query.Language,
                query.CustName);

            var expectedRunningViewModel = new RunningBetListViewModel(
                query,
                runningBetListParam,
                BetListDataTest.BetListData,
                Enumerable.Empty<GroupProductViewModel>(),
                BetListDataTest.BettypeIds,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualRunningViewModel = runningBetListHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualRunningViewModel, expectedRunningViewModel).AreEqual);
        }
    }
}
