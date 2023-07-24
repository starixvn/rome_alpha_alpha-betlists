using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Alpha.BetLists.Models.MonitoringBets;
using Alpha.BetLists.Site.ViewModels.MonitoringBets;

namespace Alpha.BetLists.Site.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using BetLists.Models;
    using Constants;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using Requests.MonitoringBets;
    using Site.Controllers;
    using Web.Models;
    using Xunit;
    using UserLevel = BetLists.Models.UserLevel;

    [Trait("Category", "MonitoringBets")]
    public class MonitoringBetsControllerTests
    {
        private readonly IMediator mediator;
        private readonly MonitoringBetsController controller;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;

        public MonitoringBetsControllerTests()
        {
            mediator = Substitute.For<IMediator>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            user.SubAccId.Returns(1);
            controller = new MonitoringBetsController(mediator, user);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
        }

        [Fact]
        public async Task Index_Always_CallsMediatorSendMonitoringBetsQuery()
        {
            // Arrange
            var message = new MonitoringBetsQuery(user);

            // Act
            await controller.Index(message);

            // Assert
            await mediator.Received().Send(message);
        }

        [Fact]
        public async Task Index_FilterNotHasData_RedirectToFilterConfiguration()
        {
            // Arrange
            var message = new MonitoringBetsQuery(user);

            var expectedResult = new MonitoringBetsViewModel(0, new List<SavedFilter>(), 0, string.Empty, string.Empty, message, null, null, null);

            mediator
                .Send(message).Returns(Task.FromResult(expectedResult));

            // Act
            var result = await controller.Index(message) as RedirectToRouteResult;

            // Assert
            Assert.Equal("FilterConfiguration", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Index_FiltersHasData_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MonitoringBetsQuery(user);

            var expectedResult = new MonitoringBetsViewModel(1, new List<SavedFilter>()
            {
                new SavedFilter(1, "Test", true, "1,2", 100, 1, "0", "0", "0", 1),
            }, 0, string.Empty, string.Empty, message, null, null, null);
            mediator
                .Send(message).Returns(Task.FromResult(expectedResult));

            // Act
            var actualView = await controller.Index(message) as ViewResult;

            // Assert
            Assert.IsType<MonitoringBetsViewModel>(actualView.Model);

        }

        [Fact]
        public void ManageFilters_Always_CallsMediatorSendManageFiltersQuery()
        {
            // Arrange
            var message = new ManageFiltersQuery(user);

            // Act
            controller.ManageFilters(message).GetAwaiter().GetResult();

            // Assert
            mediator.Received().Send(message);
        }

        [Fact]
        public async Task UpdateFilter_ValidCommand_CallsUpdateFilterCommand()
        {
            // Arrange
            var message = new UpdateFilterCommand(user);
            message.FilterId = 1;
            message.FilterName = "Default";

            // Act
            await controller.UpdateFilter(message);

            // Assert
            await mediator.Received().Send(message);
        }

        [Fact]
        public async Task UpdateFilter_InternalUser_ReturnsForbiddenResult()
        {
            // Arrange
            user.IsInternal.Returns(true);
            var message = new UpdateFilterCommand(user);
            message.FilterId = 1;
            message.FilterName = "Default";
            // Act
            var actualResult = await controller.UpdateFilter(message) as JsonResult;
            var actualData = (Result)actualResult.Data;

            // Assert
            Assert.Equal(AppResources.Resources.NotAuthorized, actualData.ErrMsg);
        }

        [Fact]
        public async Task UpdateFilter_InvalidCommand_ReturnsForbiddenResult()
        {
            // Arrange
            var message = new UpdateFilterCommand(user);
            message.FilterId = 0;
            // Act
            var actualResult = await controller.UpdateFilter(message) as JsonResult;
            var actualData = (Result)actualResult.Data;

            // Assert
            Assert.Equal(AppResources.Resources.BadRequest, actualData.ErrMsg);
        }

        [Fact]
        public async Task DeleteFilters_ValidCommand_CallsDeleteFiltersCommand()
        {
            // Arrange
            var message = new DeleteFiltersCommand(user);
            message.Ids = "1,2,3";

            // Act
            await controller.DeleteFilters(message);

            // Assert
            await mediator.Received().Send(message);
        }

        [Fact]
        public async Task DeleteFilters_InternalUser_ReturnsForbiddenResult()
        {
            // Arrange
            user.IsInternal.Returns(true);
            var message = new DeleteFiltersCommand(user);
            message.Ids = "1,2,3";

            // Act
            var actualResult = await controller.DeleteFilters(message) as JsonResult;
            var actualData = (IEnumerable<Result>)actualResult.Data;

            // Assert
            Assert.Equal(AppResources.Resources.NotAuthorized, actualData.First().ErrMsg);
        }

        [Fact]
        public async Task DeleteFilters_InvalidCommand_ReturnsForbiddenResult()
        {
            // Arrange
            var message = new DeleteFiltersCommand(user);
            message.Ids = "";
            // Act
            var actualResult = await controller.DeleteFilters(message) as JsonResult;
            var actualData = (IEnumerable<Result>)actualResult.Data;

            // Assert
            Assert.Equal(AppResources.Resources.BadRequest, actualData.First().ErrMsg);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FilterConfiguration_Always_ReturnValidPageView(bool welcome)
        {
            // Arrange & Act
            var actualView = controller.FilterConfiguration(welcome) as ViewResult;
            // Assert
            Assert.IsType<FilterConfigurationViewModel>(actualView.Model);
        }

        [Fact]
        public async Task FilterBySportsQuery_Always_CallMediatorFilterBySportsQuery()
        {
            // Arrange
            var message = new FilterBySportsQuery(user);
            // Act
            await controller.FilterBySports(message);

            // Assert
            await mediator.Received().Send(message);
        }

        [Fact]
        public async Task CheckUsernamesQuery_Always_CallMediatorCheckUsernamesQuery()
        {
            // Arrange
            var message = new CheckUsernamesQuery(user);
            // Act
            await controller.CheckUsernames(message);

            // Assert
            await mediator.Received().Send(message);
        }

        [Fact]
        public async Task GetMyWatchlistMembersQuery_Always_CallMediatorGetMyWatchlistMembersQuery()
        {
            // Arrange
            var message = new GetMyWatchlistMembersQuery(user);
            // Act
            await controller.GetMyWatchlistMembers(message);

            // Assert
            await mediator.Received().Send(message);
        }

        [Fact]
        public async Task ApplyFilterCommand_ValidCommand_CallMediatorApplyFilterCommand()
        {
            // Arrange
            var message = new ApplyFilterCommand(user);
            message.FilterId = 1;
            message.FilterName = "Default";
            message.BetType = "1,2,3";
            message.Stake = 10;
            message.Sports = new[] { new SportXml() { League = "A", Sport = "Soccer" } };
            message.FilterByMembers = 1;
            message.MyWatchlist = "0";

            // Act
            await controller.ApplyFilter(message);

            // Assert
            await mediator.Received().Send(message);
        }


        [Fact]
        public async Task ApplyFilterCommand_InternalUser_ReturnsForbiddenResult()
        {
            // Arrange
            user.IsInternal.Returns(true);
            var message = new ApplyFilterCommand(user);
            message.FilterId = 1;
            message.FilterName = "Default";

            // Act
            var actualResult = await controller.ApplyFilter(message) as JsonResult;
            var actualData = (Result)actualResult.Data;

            // Assert
            Assert.Equal(AppResources.Resources.NotAuthorized, actualData.ErrMsg);
        }


        [Fact]
        public async Task AddFilterCommand_ValidCommand_CallMediatorAddFilterCommand()
        {
            // Arrange
            var message = new AddFilterCommand(user);
            message.FilterId = 1;
            message.FilterName = "Default";
            message.BetType = "1,2,3";
            message.Stake = 10;
            message.Sports = new[] { new SportXml() { League = "A", Sport = "Soccer" } };
            message.FilterByMembers = 1;
            message.MyWatchlist = "0";
            // Act
            await controller.AddFilter(message);

            // Assert
            await mediator.Received().Send(message);
        }


        [Fact]
        public async Task AddFilterCommand_InternalUser_ReturnsForbiddenResult()
        {
            // Arrange
            user.IsInternal.Returns(true);
            var message = new AddFilterCommand(user);

            // Act
            var actualResult = await controller.AddFilter(message) as JsonResult;
            var actualData = (Result)actualResult.Data;

            // Assert
            Assert.Equal(AppResources.Resources.NotAuthorized, actualData.ErrMsg);
        }

        [Fact]
        public async Task AddFilterCommand_InvalidCommand_ReturnsForbiddenResult()
        {
            // Arrange
            var message = new AddFilterCommand(user);
            // Act
            var actualResult = await controller.AddFilter(message) as JsonResult;
            var actualData = (Result)actualResult.Data;

            // Assert
            Assert.Equal(AppResources.Resources.BadRequest, actualData.ErrMsg);
        }
    }
}
