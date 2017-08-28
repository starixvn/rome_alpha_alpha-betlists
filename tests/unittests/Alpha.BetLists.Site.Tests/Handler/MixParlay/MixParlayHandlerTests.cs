namespace Alpha.BetLists.Site.Tests.Handler.MixParlay
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.MixParlay;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.MixParlay;
    using Services;
    using Site.ViewModels.MixParlay;
    using Xunit;

    [Trait("Category", "MixParlay")]
    [Collection("BetListCollection")]
    public class MixParlayHandlerTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly MixParlayQuery message;
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;
        private readonly MixParlayHandler mixParlayHandler;

        public MixParlayHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            compareLogic = new CompareLogic();
            message = new MixParlayQuery(betlistFixture.User);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            runningBetListByBetTypeService = Substitute.For<RunningBetListByBetTypeService>();
            runningBetListByBetTypeService.GetBetList(Arg.Any<RunningBetListByBetTypeParam>()).Returns(BetListDataTest.Tickets);

            mixParlayHandler = new MixParlayHandler(
                runningBetListByBetTypeService,
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
            mixParlayHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_Always_CallGetBetListFromService()
        {
            // Arrange
            var param = new RunningBetListByBetTypeParam(
                message.ViewerId,
                message.BetType,
                message.Language,
                message.ViewerLevel);

            // Act
            mixParlayHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            runningBetListByBetTypeService.Received().GetBetList(
                Arg.Is<RunningBetListByBetTypeParam>(
                    p => compareLogic.Compare(param, p).AreEqual));
        }

        [Fact]
        public void Handle_Always_CallGetBetTypeIdsSupportScoreMapFromService()
        {
            // Act
            mixParlayHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            forecastService.Received().GetBetTypeIdsSupportScoreMap();
        }

        [Fact]
        public void Handle_Always_ReturnsMixParlayViewModel()
        {
            // Arrange
            var expectedViewModel = new MixParlayViewModel(
                message,
                new BetListData(BetListDataTest.Tickets, null),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                BetListDataTest.BettypeIds);

            // Act
            var actualViewModel = mixParlayHandler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}
