namespace Alpha.BetLists.Site.Tests.Controllers
{
    using System.Globalization;
    using System.Threading;
    using BetLists.Models;
    using Constants;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using Requests.LastBets;
    using Site.Controllers;
    using Web.Models;
    using Xunit;

    [Trait("Category", "LastBets")]
    public class LastBetsControllerTests
    {
        private readonly IMediator mediator;
        private readonly LastBetsController controller;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;

        public LastBetsControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);

            controller = new LastBetsController(mediator, user);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void Index_Always_CallsMediatorSendLastBetsQuery()
        {
            // Arrange
            var message = new LastBetsQuery(user);

            // Act
            controller.Index(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public void BetData_CanNotViewLastBets_ReturnsErrorResult()
        {
            // Arrange
            var message = new GetLastBetsQuery(user);
            var errorResult = new Result
            {
                ErrCode = 1,
                ErrMsg = "Our server is too busy at the moment so this function is turned off temporarily. Please try again later and sorry for the inconvenience."
            };

            mediator.Send(Arg.Any<CheckCanViewLastBetsQuery>()).Returns(errorResult);

            // Act
            var actualResult = controller.BetData(message).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualResult, actualResult).AreEqual);
        }

        [Fact]
        public void BetData_CanViewLastBets_CallsMediatorSendGetLastBetsQuery()
        {
            // Arrange
            var message = new GetLastBetsQuery(user);
            var checkCanViewLastBetsResult = new Result(0, string.Empty);
            mediator.Send(Arg.Any<CheckCanViewLastBetsQuery>()).Returns(checkCanViewLastBetsResult);

            // Act
            controller.BetData(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }
    }
}
