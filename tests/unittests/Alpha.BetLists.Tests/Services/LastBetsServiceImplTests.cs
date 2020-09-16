namespace Alpha.BetLists.Tests.Services
{
    using BetLists.Configurations;
    using BetLists.Criteria.LastBets;
    using BetLists.Models;
    using BetLists.Params.LastBets;
    using BetLists.Services;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using TestData;
    using Xunit;

    [Trait("Category", "LastBets")]
    [Collection("FanexDataCollection")]
    public class LastBetsServiceImplTests
    {
        private readonly LastBetsServiceImpl lastBetsService;
        private readonly CompareLogic compareLogic;

        public LastBetsServiceImplTests(FanexDataFixture dataFixture)
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["EnableBetsListMonitoring"] = "true",
                ["UsersAlwaysAllowAccessBetsListMonitoring"] = "1176086"
            };

            dataFixture.DynamicRepository
                .FetchAsync<TicketDto>(Arg.Any<GetLastBetsCriteria>())
                .Returns(LastBetsTestData.AllTickets);

            dataFixture.DynamicRepository
                .Fetch<MatchInfo>(Arg.Any<GetRunningOutrightMatchCriteria>())
                .Returns(LastBetsTestData.RunningOutrightMatch);

            dataFixture.DynamicRepository
                .Fetch<MatchInfo>(Arg.Any<GetRunningOtherMatchCriteria>())
                .Returns(LastBetsTestData.RunningOtherMatch);

            lastBetsService = new LastBetsServiceImpl(dataFixture.DynamicRepository, dataFixture.CacheService);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void CanViewLastBets_IsInternal_ReturnsTrue()
        {
            // Arrange
            var param = new CheckCanViewLastBetsParam(0, true, false, Enumerable.Empty<int>());

            // Act
            var actualCanViewLastBets = lastBetsService.CanViewLastBets(param);

            // Assert
            Assert.True(actualCanViewLastBets);
        }

        [Fact]
        public void CanViewLastBets_NotInternalAndEnableConfig_ReturnTrue()
        {
            // Arrange
            var param = new CheckCanViewLastBetsParam(0, false, true, Enumerable.Empty<int>());

            // Act
            var actualCanViewLastBets = lastBetsService.CanViewLastBets(param);

            // Assert
            Assert.True(actualCanViewLastBets);
        }

        [Fact]
        public void CanViewLastBets_CustIdInAlwaysAllowAccessList_ReturnTrue()
        {
            // Arrange
            var param = new CheckCanViewLastBetsParam(2, false, false, new List<int> { 1, 2, 3 });

            // Act
            var actualCanViewLastBets = lastBetsService.CanViewLastBets(param);

            // Assert
            Assert.True(actualCanViewLastBets);
        }

        [Fact]
        public void CanViewLastBets_CustIdNotInAlwaysAllowAccessList_ReturnFalse()
        {
            // Arrange
            var param = new CheckCanViewLastBetsParam(4, false, false, new List<int> { 1, 2, 3 });

            // Act
            var actualCanViewLastBets = lastBetsService.CanViewLastBets(param);

            // Assert
            Assert.False(actualCanViewLastBets);
        }
    }
}