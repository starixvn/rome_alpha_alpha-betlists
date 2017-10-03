namespace Alpha.BetLists.Site.Tests.Handler.CombinationMixParlay
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.CombinationMixParlay;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.CombinationMixParlay;
    using Services;
    using Site.ViewModels.CombinationMixParlay;
    using Xunit;

    [Trait("Category", "CombinationMixParlay")]
    [Collection("BetListCollection")]
    public class CombinationMixParlayHandlerTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly CombinationMixParlayQuery message;
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;
        private readonly CombinationMixParlayHandler handler;

        public CombinationMixParlayHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            compareLogic = new CompareLogic();
            message = new CombinationMixParlayQuery(betlistFixture.User);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            runningBetListByBetTypeService = Substitute.For<RunningBetListByBetTypeService>();
            runningBetListByBetTypeService.GetBetList(Arg.Any<RunningBetListByBetTypeParam>()).Returns(BetListDataTest.Tickets);

            handler = new CombinationMixParlayHandler(
                runningBetListByBetTypeService,
                forecastService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MonitorService,
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
            betlistFixture.MonitorService.Received().WriteLog(
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
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            runningBetListByBetTypeService.Received().GetBetList(Arg.Is<RunningBetListByBetTypeParam>(p => compareLogic.Compare(param, p).AreEqual));
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
        public void Handle_Always_ReturnsCombinationMixParlayViewModel()
        {
            // Arrange
            var expectedViewModel = new CombinationMixParlayViewModel(
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
