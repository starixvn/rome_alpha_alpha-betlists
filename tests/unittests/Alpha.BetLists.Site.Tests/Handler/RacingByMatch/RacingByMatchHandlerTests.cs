namespace Alpha.BetLists.Site.Tests.Handler.RacingByMatch
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.RacingByMatch;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.RacingByMatch;
    using Services;
    using Site.ViewModels.RacingByMatch;
    using Xunit;

    [Trait("Category", "RacingByMatch")]
    [Collection("BetListCollection")]
    public class RacingByMatchHandlerTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly RacingByMatchQuery message;
        private readonly ForecastService forecastService;
        private readonly RacingByMatchService racingByMatchService;
        private readonly RacingByMatchHandler handler;

        public RacingByMatchHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            compareLogic = new CompareLogic();
            message = new RacingByMatchQuery(betlistFixture.User);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            racingByMatchService = Substitute.For<RacingByMatchService>();
            racingByMatchService.GetBetList(Arg.Any<RacingByMatchParam>()).Returns(BetListDataTest.Tickets);

            handler = new RacingByMatchHandler(
                racingByMatchService,
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
            var param = new RacingByMatchParam(
                message.ViewerId,
                message.BetType,
                message.MatchId,
                message.Language,
                message.ViewerLevel);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            racingByMatchService.Received().GetBetList(
                Arg.Is<RacingByMatchParam>(
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
        public void Handle_Always_ReturnsRacingByMatchViewModel()
        {
            // Arrange
            var expectedViewModel = new RacingByMatchViewModel(
                message,
                new BetListData(BetListDataTest.Tickets, null),
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
