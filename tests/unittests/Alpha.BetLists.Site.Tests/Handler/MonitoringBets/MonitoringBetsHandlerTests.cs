using System.Threading.Tasks;
using Alpha.BetLists.Models.MonitoringBets;

namespace Alpha.BetLists.Site.Tests.Handler.MonitoringBets
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Alpha.Web.Models;
    using AlphaPlus.User.Models;
    using BetLists.Models;
    using Constants;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.MonitoringBets;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params.MonitoringBets;
    using Requests.MonitoringBets;
    using Services;
    using Site.ViewModels.MonitoringBets;
    using AlphaPlus.SmartPunter.Services;
    using Xunit;
    using UserLevel = BetLists.Models.UserLevel;

    [Trait("Category", "MonitoringBets")]
    [Collection("BetListCollection")]
    public class MonitoringBetsHandlerTests
    {
        private readonly MonitoringBetsHandler handler;
        private readonly MonitoringBetsService service;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly EncryptionService encryptionService;
        private readonly ISmartPunterBetListMonitor monitorService;
        private readonly IEnumerable<TicketDto> tickets;
        private readonly IEnumerable<SavedFilter> savedFilters;
        private readonly IEnumerable<TicketDto> newTickets = new List<TicketDto> {
            new TicketDto { TransId = 30 },
            new TicketDto { TransId = 20 }
        };

        private readonly IUserSession userSession;
        private readonly UsernameSuggestionService usernameSuggestionService;

        public MonitoringBetsHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            userSession = Substitute.For<IUserSession>();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            service = Substitute.For<MonitoringBetsService>();

            tickets = CreateTickets();
            service.GetLastFilteringBets(Arg.Any<GetLastFilteringBetsParams>()).Returns(tickets);
            savedFilters = new List<SavedFilter>()
            {
                new SavedFilter(1, "Test", true, false, "1,2", 100, 1, "0", "0", "0", 1),
            };
            service.GetSavedFilters(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<AddFilterParam>()).Returns(savedFilters);

            encryptionService = Substitute.For<EncryptionService>();
            encryptionService.CfsEncode(Arg.Any<string>(), Arg.Any<int>()).Returns("EncodedString");

            monitorService = Substitute.For<ISmartPunterBetListMonitor>();
            usernameSuggestionService = Substitute.For<UsernameSuggestionService>();

            handler = new MonitoringBetsHandler(
                userSession,
                service,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                monitorService,
                this.betlistFixture.IpService,
                encryptionService,
                betlistFixture.IpInfoService);

            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public async Task HandleMonitoringBetsQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new MonitoringBetsQuery(user);
            var expectedViewModel = new MonitoringBetsViewModel(
                1,
                savedFilters,
                false,
                tickets.First().TransId,
                "EncodedString",
                "EncodedString",
                message,
                new BetListData(tickets, null),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public async Task HandleManageFiltersQuery_Always_CallsGetFilters()
        {
            // Arrange
            var message = new ManageFiltersQuery(user);

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().GetFilters(message.CustId, message.SubAccId, null);
        }

        [Fact]
        public async Task HandleUpdateFilterCommand_Always_CallsUpdateFilter()
        {
            // Arrange
            var message = new UpdateFilterCommand(user);

            var param = new UpdateFilterParam(
                message.ViewerId,
                message.SubAccId,
                1,
                "Default",
                true,
                message.LanguageKey,
                message.IP,
                false);

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().UpdateFilter(Arg.Any<UpdateFilterParam>());
        }

        [Fact]
        public async Task HandleDeleteFiltersCommand_HasCanDeleteFilters_CallsDeleteFilters()
        {
            // Arrange
            var message = new DeleteFiltersCommand(user);
            message.Ids = "1,2,3";

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().DeleteFilter(Arg.Any<DeleteFilterParam>());
        }

        [Fact]
        public async Task HandleDeleteFiltersCommand_NotHasCanDeleteFilters_NotCallsDeleteFilters()
        {
            // Arrange
            var message = new DeleteFiltersCommand(user);
            message.Ids = "0";

            // Act
            await handler.Handle(message);

            // Assert
            await service.DidNotReceive().DeleteFilter(Arg.Any<DeleteFilterParam>());
        }

        [Fact]
        public async Task HandleFilterBySportsQuery_Always_CallsGetSports()
        {
            // Arrange
            var message = new FilterBySportsQuery(user)
            {
                GroupSport = 1
            };

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().GetSports(message.GroupSport);
        }


        [Fact]
        public async Task HandleGetMyWatchlistMembersQuery_Always_CallsGetMyWatchlistMembers()
        {
            // Arrange
            var message = new GetMyWatchlistMembersQuery(user);

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().GetMyWatchlistMembers(message.ViewerId);
        }

        [Fact]
        public async Task ApplyFilterCommand_Always_ReturnSuccess()
        {
            // Arrange
            var message = new ApplyFilterCommand(user);

            // Act
            var actualResult = await handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(new Result(), actualResult).AreEqual);
        }


        [Fact]
        public async Task AddUnsavedFilterCommand_HasUnsavedFilter_CallsAddFilter()
        {
            // Arrange
            var message = new AddUnsavedFilterCommand(user);
            var unsavedFilter = new AddFilterParam(1, 2, 1, "UnsavedFilter", false, "1", 10,
                                new[] { new SportXml() { League = "Basketball", Sport = "Soccer" } },
                                1, "0", "0", "0", "0", 1, "En", "1.2.3.4", false);
            service.AddFilter(Arg.Any<AddFilterParam>()).Returns(new Result());

            // Act
            var actualResult = await handler.Handle(message);

            // Assert
            await service.Received().AddFilter(Arg.Any<AddFilterParam>());
        }

        [Fact]
        public async Task AddUnsavedFilterCommand_NotHasUnsavedFilter_NotCallsAddFilter()
        {
            // Arrange
            var message = new AddUnsavedFilterCommand(user);

            // Act
            var actualResult = await handler.Handle(message);

            // Assert
            await service.DidNotReceive().AddFilter(Arg.Any<AddFilterParam>());
        }


        [Fact]
        public async Task AddFilterCommand_Always_CallsAddFilter()
        {
            // Arrange
            var message = new AddFilterCommand(user);

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().AddFilter(Arg.Any<AddFilterParam>());
        }

        [Fact]
        public async Task HandleGetLastFilteringBetsQuery_Always_ReturnsCorrectViewModel()
        {
            // Arrange

            var message = new GetLastFilteringBetsQuery(user);

            var expectedViewModel = new FilteringBetDataViewModel(30,
                "EncodedString",
                "EncodedString",
                message,
                new BetListData(tickets, null),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualViewModel = await handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public async Task HandleMonitoringBetsQuery_FirstLoad_AlwaysWriteLog()
        {
            // Arrange
            var message = new MonitoringBetsQuery(user);
            var expected = new List<SavedFilter>()
            {
                new SavedFilter(1, "Test", true, false, "1,2", 100, 1, "0", "0", "0", 1),
            };

            service.GetSavedFilters(message.ViewerId, message.SubAccId, null).Returns(Task.FromResult(expected).Result);
            // Act
            await handler.Handle(message);

            // Assert
            monitorService.Received().WriteLog(
                Arg.Any<int>(),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(tickets)));
        }


        [Fact]
        public void HandleGetLastFilteringBetsQuery_AutoLoadAndHaveNewTickets_WriteLog()
        {
            // Arrange
            var message = new GetLastFilteringBetsQuery(user);
            message.LatestTransId = 10;

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            monitorService.Received().WriteLog(
                Arg.Any<int>(),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(newTickets)));
        }

        [Fact]
        public void HandleGetLastFilteringBetsQuery_AutoLoadNotHaveNewTickets_NotCallWriteLog()
        {
            // Arrange
            var message = new GetLastFilteringBetsQuery(user);
            message.LatestTransId = 30;

            // Act
            handler.Handle(message).GetAwaiter().GetResult();

            // Assert
            monitorService.DidNotReceive().WriteLog(
                Arg.Any<int>(),
                Arg.Any<IList<ITicket>>());
        }

        private IEnumerable<TicketDto> CreateTickets()
        {
            var result = new List<TicketDto>();
            result.AddRange(newTickets);
            result.Add(new TicketDto { TransId = 10 });
            return result;
        }
    }
}
