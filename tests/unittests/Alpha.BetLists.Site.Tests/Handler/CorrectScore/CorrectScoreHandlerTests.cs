namespace Alpha.BetLists.Site.Tests.Handler.CorrectScore
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.CorrectScore;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.CorrectScore;
    using Services;
    using Site.ViewModels.CorrectScore;
    using Xunit;

    [Trait("Category", "CorrectScore")]
    [Collection("BetListCollection")]
    public class CorrectScoreHandlerTests
    {
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly CorrectScoreQuery message;
        private readonly ForecastService forecastService;
        private readonly CorrectScoreService correctScoreService;
        private readonly CorrectScoreHandler handler;

        public CorrectScoreHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            compareLogic = new CompareLogic();
            message = new CorrectScoreQuery(betlistFixture.User);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            correctScoreService = Substitute.For<CorrectScoreService>();
            correctScoreService.GetBetList(Arg.Any<CorrectScoreParam>()).Returns(BetListDataTest.Tickets);

            handler = new CorrectScoreHandler(
                correctScoreService,
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
            var correctScoreParam = new CorrectScoreParam(
                message.ViewerId,
                message.Language,
                message.ViewerLevel);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            correctScoreService.Received().GetBetList(Arg.Is<CorrectScoreParam>(p => compareLogic.Compare(correctScoreParam, p).AreEqual));
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
        public void Handle_Always_ReturnsCorrectScoreViewModel()
        {
            // Arrange
            var expectedViewModel = new CorrectScoreViewModel(
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
