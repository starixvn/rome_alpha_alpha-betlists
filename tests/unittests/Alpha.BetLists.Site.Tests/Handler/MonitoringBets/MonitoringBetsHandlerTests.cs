using System.Threading.Tasks;
using Alpha.BetLists.Models.MonitoringBets;
using Alpha.BetLists.Site.Constants;
using Alpha.BetLists.Site.ViewModels.LastBets;
using Sunplus.Agent.Model.Entities;

namespace Alpha.BetLists.Site.Tests.Handler.MonitoringBets
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using BetLists.Models;
    using Constants;
    using Fanex.BetList.Core.Entities;
    using Fanex.Caching;
    using Fixtures;
    using Handlers.MonitoringBets;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params.MonitoringBets;
    using Requests.MonitoringBets;
    using Services;
    using Site.ViewModels.MonitoringBets;
    using SmartPunter.Services;
    using Web.Models;
    using Xunit;
    using UserLevel = BetLists.Models.UserLevel;

    [Trait("Category", "MonitoringBets")]
    [Collection("BetListCollection")]
    public class MonitoringBetsHandlerTests
    {
        private const string LatestTransIdCacheKey = "Alpha.Reports.Handlers.MonitoringBets.LatestTransIdCacheKey";
        private readonly MonitoringBetsHandler handler;
        private readonly MonitoringBetsService service;
        private readonly LastBetsService lastBetsService;
        private readonly IUser user;
        private readonly CompareLogic compareLogic;
        private readonly BetListFixture betlistFixture;
        private readonly EncryptionService encryptionService;
        private readonly ISmartPunterBetListMonitor monitorService;
        private readonly IEnumerable<TicketDto> tickets;
        private readonly IUserSession userSession;

        public MonitoringBetsHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            userSession = Substitute.For<IUserSession>();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            service = Substitute.For<MonitoringBetsService>();
            lastBetsService = Substitute.For<LastBetsService>();

            tickets = CreateTickets();
            service.GetLastFilteringBets(Arg.Any<GetLastFilteringBetsParams>()).Returns(tickets);

            encryptionService = Substitute.For<EncryptionService>();
            encryptionService.CfsEncode(Arg.Any<string>(), Arg.Any<int>()).Returns("EncodedString");


            monitorService = Substitute.For<ISmartPunterBetListMonitor>();

            handler = new MonitoringBetsHandler(userSession,
                service,
                lastBetsService,
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
                0,
                new List<SavedFilter>(),
                0,
                "EncodedString",
                "EncodedString",
                message,
                null,
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
            await service.Received().GetFilters(
                message.CustId,
                message.SubAccId);
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
                message.IP);

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().UpdateFilter(Arg.Any<UpdateFilterParam>());
        }

        [Fact]
        public async Task HandleDeleteFiltersCommand_Always_CallsDeleteFilters()
        {
            // Arrange
            var message = new DeleteFiltersCommand(user);

            var param = new DeleteFiltersParam(
                message.ViewerId,
                message.SubAccId,
                "1,2,3",
                message.IP);

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().DeleteFilters(Arg.Any<DeleteFiltersParam>());
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
        public async Task HandleCheckUsernamesQuery_Always_CallsGetSports()
        {
            // Arrange
            var message = new CheckUsernamesQuery(user)
            {
                Usernames = "member01,member02"
            };

            // Act
            await handler.Handle(message);

            // Assert
            await service.Received().CheckUsernames(message.ViewerId, message.Usernames);
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
        public async Task HandleApplyFilterCommand_Always_CallsBuildSavedFilter()
        {
            // Arrange
            var message = new ApplyFilterCommand(user)
            {
                FilterId = 1,
                FilterName = "Default",
                BetType = "1,2,3",
                Stake = 10,
                Sports = new[] { new SportXml() { League = "Basketball", Sport = "Soccer" } },
                LiveStatus = 1,
                InputList = "0",
                MyWatchlist = "0",
                FilterByMembers = 1
            };

            var param = new AddFilterParam(
                message.ViewerId,
                message.SubAccId,
                message.FilterName,
                message.IsDefault,
                message.BetType,
                message.Stake,
                message.Sports,
                message.LiveStatus,
                message.InputList,
                message.MyWatchlist,
                message.FilterByMembers,
                message.LanguageKey,
                message.IP);
            // Act
            await handler.Handle(message);

            // Assert
            service.Received().BuildSavedFilter(Arg.Any<AddFilterParam>());
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
                new SavedFilter(1,"Test",true,"1,2",100,1,"0","0","0",1),
            };

            service.GetSavedFilters(message.ViewerId, message.SubAccId).Returns(Task.FromResult(expected).Result);
            // Act
            await handler.Handle(message);

            // Assert
            monitorService.Received().WriteLog(
                Arg.Any<int>(),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(tickets)));
        }

        [Fact]
        public async Task HandleGetLastFilteringBetsQuery_AutoLoadAndHaveNewTickets_WriteLog()
        {
            // Arrange
            var message = new GetLastFilteringBetsQuery(user);
            
            message.LatestTransId = 10;

            var expectedWriteLogTickets = new List<ITicket> {
                new TicketDto { TransId = 30 },
                new TicketDto { TransId = 20 }
            };

            // Act
            await handler.Handle(message);

            // Assert
            monitorService.Received().WriteLog(
                Arg.Any<int>(),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(expectedWriteLogTickets)));
        }

        //[Fact]
        //public void HandleGetMonitoringBetsQuery_AutoLoadNotHaveNewTickets_NotCallWriteLog()
        //{
        //    // Arrange
        //    var message = new GetMonitoringBetsQuery(user);
        //    message.IsAutoLoad = true;
        //    message.LatestTransId = 30;

        //    // Act
        //    handler.Handle(message).GetAwaiter().GetResult();

        //    // Assert
        //    monitorService.DidNotReceive().WriteLog(
        //        Arg.Any<int>(),
        //        Arg.Any<IList<ITicket>>());
        //}

        private IEnumerable<TicketDto> CreateTickets()
        {
            var result = new List<TicketDto> {
                new TicketDto { TransId = 30 },
                new TicketDto { TransId = 20 },
                new TicketDto { TransId = 10 }
            };

            return result;
        }
    }
}
