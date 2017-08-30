namespace Alpha.BetLists.Site.Tests.Handler.BetListByBetTypeAndMatch
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.BetListByBetTypeAndMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.BetListByBetTypeAndMatch;
    using Services;
    using Site.ViewModels.BetListByBetTypeAndMatch;
    using Xunit;

    [Trait("Category", "BetListByBetTypeAndMatch")]
    [Collection("BetListCollection")]
    public class BetListByBetTypeAndMatchHandlerTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly BetListByBetTypeAndMatchQuery message;
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService;
        private readonly BetListByBetTypeAndMatchHandler handler;

        public BetListByBetTypeAndMatchHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            compareLogic = new CompareLogic();
            message = new BetListByBetTypeAndMatchQuery(betlistFixture.User);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            runningBetListByBetTypeAndMatchService = Substitute.For<RunningBetListByBetTypeAndMatchService>();
            runningBetListByBetTypeAndMatchService
                .GetBetList(Arg.Any<RunningBetListByBetTypeAndMatchParam>())
                .Returns(BetListDataTest.BetListData);

            handler = new BetListByBetTypeAndMatchHandler(
                runningBetListByBetTypeAndMatchService,
                forecastService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MoniterService,
                betlistFixture.IpService);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        public void Handle_Always_CallWriteLog(int custId, int expectedId)
        {
            // Arrange
            message.CustId = custId;

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            message.BetType = 13;
            var param = new RunningBetListByBetTypeAndMatchParam(
                message.MatchId,
                message.ViewerId,
                message.BetType,
                message.Language,
                message.IsLive,
                message.BetTeam,
                message.ViewerLevel);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            runningBetListByBetTypeAndMatchService.Received().GetBetList(
                Arg.Is<RunningBetListByBetTypeAndMatchParam>(
                    p => compareLogic.Compare(param, p).AreEqual));
        }

        [Fact]
        public void Handle_Always_CallGetBetTypeIdsSupportScoreMapFromService()
        {
            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            forecastService.Received().GetBetTypeIdsSupportScoreMap();
        }

        [Fact]
        public void Handle_Always_ReturnsBetListByBetTypeAndMatchViewModel()
        {
            // Arrange
            var expectedViewModel = new BetListByBetTypeAndMatchViewModel(
                message,
                BetListDataTest.BetListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                BetListDataTest.BettypeIds);

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}
