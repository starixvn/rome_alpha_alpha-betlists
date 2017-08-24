namespace Alpha.BetLists.Site.Tests.Handler.DailyStatement
{
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.DailyStatement;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.DailyStatement;
    using Services;
    using Site.ViewModels.DailyStatement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "DailyStatement")]
    [Collection("BetListCollection")]
    public class DailyStatementHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly DailyStatementHandler dailyStatementHandler;
        private readonly DailyStatementService dailyStatementService;
        private readonly ForecastService forecastService;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;

        public DailyStatementHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            dailyStatementService = Substitute.For<DailyStatementService>();
            dailyStatementService.GetBetList(Arg.Any<DailyStatementParam>()).Returns(BetListDataTest.BetListData);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            dailyStatementHandler = new DailyStatementHandler(
                dailyStatementService,
                forecastService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MoniterService,
                betlistFixture.IpService);

            user = betlistFixture.User;
            compareLogic = new CompareLogic();
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        public void Handle_Always_CallsWriteLog(int custId, int expectedId)
        {
            // Arrange
            var query = new DailyStatementQuery(user)
            {
                CustId = custId
            };

            // Act
            dailyStatementHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallsGetBetListFromService()
        {
            // Arrange
            var query = CreateDailyStatementQuery();

            var expectedParam = new DailyStatementParam(1, DateTime.Today, 1, "en");

            // Act
            dailyStatementHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            dailyStatementService.Received().GetBetList(Arg.Is<DailyStatementParam>(p => compareLogic.Compare(expectedParam, p).AreEqual));
        }

        [Fact]
        public void CreateDailyStatementParam_Always_ReturnsDailyStatementParam()
        {
            // Arrange
            var query = CreateDailyStatementQuery();

            var expectedParam = new DailyStatementParam(1, DateTime.Today, 1, "en");

            // Act
            var actualParam = DailyStatementHandler.CreateDailyStatementParam(query);

            // Assert
            Assert.True(compareLogic.Compare(expectedParam, actualParam).AreEqual);
        }

        [Fact]
        public void Hanlde_Always_ReturnsDailyStatementViewModel()
        {
            // Arrange
            var query = CreateDailyStatementQuery();
            var expectedViewModel = new DailyStatementViewModel(
                query,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                BetListDataTest.BettypeIds);

            // Act
            var viewModel = dailyStatementHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(viewModel, expectedViewModel).AreEqual);
        }

        private DailyStatementQuery CreateDailyStatementQuery()
            => new DailyStatementQuery(user)
            {
                CustId = 1,
                CustName = "CustName",
                WinLossDate = DateTime.Today,
                TransType = 1,
                Language = "en"
            };
    }
}
