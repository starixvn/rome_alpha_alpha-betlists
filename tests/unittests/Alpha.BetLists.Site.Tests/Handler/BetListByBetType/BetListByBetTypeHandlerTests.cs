namespace Alpha.BetLists.Site.Tests.Handler.BetListByBetType
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Helpers;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fanex.RemotingRef;
    using Fixtures;
    using Handlers.BetListByBetType;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.BetListByBetType;
    using Services;
    using Site.ViewModels.BetListByBetType;
    using Xunit;

    [Trait("Category", "BetListByBetType")]
    [Collection("BetListCollection")]
    public class BetListByBetTypeHandlerTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly BetListByBetTypeQuery message;
        private readonly ForecastService forecastService;
        private readonly RunningBetListByBetTypeService runningBetListByBetTypeService;
        private readonly BetListByBetTypeHandler handler;

        public BetListByBetTypeHandlerTests(BetListFixture betlistFixture)
        {
            compareLogic = new CompareLogic();
            this.betlistFixture = betlistFixture;
            message = new BetListByBetTypeQuery(betlistFixture.User);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            runningBetListByBetTypeService = Substitute.For<RunningBetListByBetTypeService>();
            runningBetListByBetTypeService.GetBetList(Arg.Any<RunningBetListByBetTypeParam>()).Returns(BetListDataTest.Tickets);

            handler = new BetListByBetTypeHandler(
                runningBetListByBetTypeService,
                forecastService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MonitorService,
                betlistFixture.IpService,
                betlistFixture.IpInfoService);

            var cacheRef = Substitute.For<IRemotingRefService>();
            cacheRef.GetBetTypeName(Arg.Any<string>(), Arg.Any<string>()).Returns("abc");
            CacheRefServiceHelper.Instance = cacheRef;
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
            message.BetType = 13;
            var param = new RunningBetListByBetTypeParam(
                message.ViewerId,
                message.BetType,
                message.Language,
                message.ViewerLevel);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            runningBetListByBetTypeService.Received().GetBetList(
                Arg.Is<RunningBetListByBetTypeParam>(
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
        public void Handle_Always_ReturnsBetListByBetTypeViewModel()
        {
            // Arrange
            var expectedViewModel = new BetListByBetTypeViewModel(
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
