namespace Alpha.BetLists.Tests.Services
{
    using System.Threading.Tasks;
    using Alpha.BetLists.Criteria.LastBets;
    using Alpha.BetLists.Models.MonitoringBets;
    using BetLists.Configurations;
    using BetLists.Criteria.MonitoringBets;
    using BetLists.Models;
    using BetLists.Params.MonitoringBets;
    using BetLists.Services;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System.Collections.Specialized;
    using TestData;
    using Xunit;

    [Trait("Category", "MonitoringBets")]
    [Collection("FanexDataCollection")]
    public class MonitoringBetsServiceImplTests
    {
        private readonly MonitoringBetsServiceImpl monitoringBetsService;
        private readonly LastBetsService lastBetsService;
        private readonly FanexDataFixture dataFixture;

        private readonly CompareLogic compareLogic;

        public MonitoringBetsServiceImplTests(FanexDataFixture dataFixture)
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["EnableBetsListMonitoring"] = "true",
                ["UsersAlwaysAllowAccessBetsListMonitoring"] = "1176086"
            };
            lastBetsService = Substitute.For<LastBetsService>();
            this.dataFixture = dataFixture;

            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetLastFilteringBetsCriteria>())
                .Returns(LastBetsTestData.AllTickets);

            dataFixture.DynamicRepository
                .Fetch<MatchInfo>(Arg.Any<GetRunningOutrightMatchCriteria>())
                .Returns(LastBetsTestData.RunningOutrightMatch);

            dataFixture.DynamicRepository
                .Fetch<MatchInfo>(Arg.Any<GetRunningOtherMatchCriteria>())
                .Returns(LastBetsTestData.RunningOtherMatch);

            monitoringBetsService = new MonitoringBetsServiceImpl(dataFixture.PartialUMService, lastBetsService, dataFixture.DynamicRepository);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public async Task GetSavedFilters_ValidCriteria_CallsDynamicRepository()
        {
            // Arrange & Act
            await monitoringBetsService.GetSavedFilters(1, 1);

            // Assert
            await dataFixture.DynamicRepository.Received().FetchAsync<FilterConfiguration>(Arg.Any<GetFiltersCriteria>());
        }

        [Fact]
        public async Task GetFilters_ValidCriteria_CallsDynamicRepository()
        {
            // Arrange & Act
            await monitoringBetsService.GetFilters(1, 1);

            // Assert
            await dataFixture.DynamicRepository.Received().FetchAsync<FilterConfiguration>(Arg.Any<GetFiltersCriteria>());
        }

        [Fact]
        public async Task AddFilter_Always_CallsDynamicRepository()
        {
            // Arrange

            var param = new AddFilterParam(0, 1, "New filter", true, "0", 10,
                new[] { new SportXml() { League = "A", Sport = "Soccer" } },
                1, "0", "0", 1, "EN", "1.2.3.4");
            // Act
            await monitoringBetsService.AddFilter(param);

            // Assert
            await dataFixture.DynamicRepository.Received().ExecuteAsync(Arg.Any<AddFilterCommand>());
        }

        [Fact]
        public async Task UpdateFilter_Always_CallsDynamicRepository()
        {
            // Arrange
            var param = new UpdateFilterParam(0, 1,
                1, "", true,
                "EN", "1.2.3.4");
            // Act
            await monitoringBetsService.UpdateFilter(param);

            // Assert
            await dataFixture.DynamicRepository.Received().ExecuteAsync(Arg.Any<UpdateFilterCommand>());
        }

        [Fact]
        public async Task DeleteFilters_ValidParam_CallsDynamicRepository()
        {
            // Arrange 
            var param = new DeleteFiltersParam(1, 1,
                "1", "1.2.3.4");
            // Act
            await monitoringBetsService.DeleteFilters(param);

            // Assert
            dataFixture.DynamicRepository.ReceivedWithAnyArgs();

        }

        [Fact]
        public async Task DeleteFilters_InvalidParam_NotCallsDynamicRepository()
        {
            // Arrange &
            var param = new DeleteFiltersParam(1, 1,
                "", "1.2.3.4");
            // Act
            await monitoringBetsService.DeleteFilters(param);

            // Assert
            dataFixture.DynamicRepository.DidNotReceiveWithAnyArgs();

        }

        [Fact]
        public async Task GetSports_Always_CallsDynamicRepository()
        {
            // Arrange & Act
            await monitoringBetsService.GetSports(1);

            // Assert
            await dataFixture.DynamicRepository.Received().FetchAsync<League>(Arg.Any<GetLeaguesCriteria>());
        }

        [Fact]
        public async Task CheckUsernames_Always_CallsDynamicRepository()
        {
            // Arrange & Act
            await monitoringBetsService.CheckUsernames(1,"member01");

            // Assert
            await dataFixture.DynamicRepository.Received().FetchAsync<MemberErrCode>(Arg.Any<CheckUsernamesCriteria>());
        }

        [Fact]
        public async Task GetMyWatchlistMembers_Always_CallsDynamicRepository()
        {
            // Arrange & Act
            await monitoringBetsService.GetMyWatchlistMembers(1);

            // Assert
            await dataFixture.DynamicRepository.Received().FetchAsync<MyWatchlistMember>(Arg.Any<GetMonitoredWatchlistMembersCriteria>());
        }

        [Fact]
        public async Task GetLastFilteringBets_Always_CallsDynamicRepository()
        {
            // Arrange & Act
            var param = new GetLastFilteringBetsParams(1,1,"0", "0", "0", "0",1,10,10);
            await monitoringBetsService.GetLastFilteringBets(param);

            // Assert
            await lastBetsService.Received().GetLastBets(Arg.Any<GetLastFilteringBetsCriteria>(), AppSettings.RunningMatchCacheTime);
        }
    }
}