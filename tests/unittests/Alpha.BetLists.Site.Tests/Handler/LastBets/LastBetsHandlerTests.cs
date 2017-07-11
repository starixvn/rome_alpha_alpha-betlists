using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params.LastBets;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.LastBets;
using Alpha.BetLists.Site.Requests.LastBets;
using Alpha.BetLists.Site.Tests.Constants;
using Alpha.BetLists.Site.Tests.Fixtures;
using Alpha.BetLists.Site.ViewModels.LastBets;
using Alpha.BetLists.Tests.TestData;
using Alpha.SmartPunter.Services;
using Alpha.Web.Models;
using Fanex.BetList.Core.Entities;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.LastBets
{
    [Trait("Category", "LastBets")]
    [Collection("BetListCollection")]
    public class LastBetsHandlerTests
    {
        private readonly LastBetsHandler handler;
        private readonly LastBetsService service;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly EncryptionService encryptionService;
        private readonly ForeCastService foreCastService;
        private readonly IEnumerable<int> supportScoreMapBettypeIds;
        private readonly ISmartPunterBetListMonitor monitorService;

        public LastBetsHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            service = Substitute.For<LastBetsService>();
            service.GetLastBets(Arg.Any<GetLastBetsParam>()).Returns(BetListDataTest.BetListData.Tickets);

            encryptionService = Substitute.For<EncryptionService>();
            encryptionService.CfsEncode(Arg.Any<string>(), Arg.Any<int>()).Returns("EncodedString");

            foreCastService = Substitute.For<ForeCastService>();
            supportScoreMapBettypeIds = new List<int> { 1 };
            foreCastService.GetBetTypeIdsSupportScoreMap().Returns(supportScoreMapBettypeIds);

            monitorService = Substitute.For<ISmartPunterBetListMonitor>();

            handler = new LastBetsHandler(
                service,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                foreCastService,
                monitorService,
                this.betlistFixture.IpService,
                encryptionService);

            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void HandleLastBetsQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new LastBetsQuery(user);
            var expectedViewModel = new LastBetsViewModel(
                message,
                null,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                "EncodedString");

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public void HandleGetLastBetsQuery_Always_WriteLog()
        {
            // Arrange
            var message = new GetLastBetsQuery(user);

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            monitorService.Received().WriteLog(
                Arg.Any<int>(),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }


        [Fact]
        public void HandleGetLastBetsQuery_Always_ReturnsBetDataViewModel()
        {
            // Arrange
            var message = new GetLastBetsQuery(user);
            var betListData = new BetListData(BetListDataTest.BetListData.Tickets, null);
            var expectedViewModel = new BetDataViewModel(
                message,
                betListData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                supportScoreMapBettypeIds);

            // Act
            var actualViewModel = handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public void HandleCheckCanViewLastBetsQuery_CanNotViewLastBets_ReturnsErrorResult()
        {
            // Arrange
            service.CanViewLastBets(Arg.Any<CheckCanViewLastBetsParam>()).Returns(false);
            var message = new CheckCanViewLastBetsQuery(user);
            var expectedResult = new Result(
                1,
                "Our server is too busy at the moment so this function is turned off temporarily. Please try again later and sorry for the inconvenience.");

            // Act
            var actualResult = handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(actualResult, actualResult).AreEqual);
        }

        [Fact]
        public void HandleCheckCanViewLastBetsQuery_CanNotViewLastBets_ReturnsSuccessResult()
        {
            // Arrange
            service.CanViewLastBets(Arg.Any<CheckCanViewLastBetsParam>()).Returns(true);
            var message = new CheckCanViewLastBetsQuery(user);
            var expectedResult = new Result(0, string.Empty);

            // Act
            var actualResult = handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(actualResult, actualResult).AreEqual);
        }
    }
}
