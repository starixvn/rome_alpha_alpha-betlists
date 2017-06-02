using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Alpha.BetLists.Configurations;
using Alpha.BetLists.Criteria.LastBets;
using Alpha.BetLists.Models;
using Alpha.BetLists.Params.LastBets;
using Alpha.BetLists.Services;
using Alpha.BetLists.Tests.Constants;
using Alpha.BetLists.Tests.Fixtures;
using Alpha.BetLists.Tests.TestData;
using Fanex.Data.Repository;
using KellermanSoftware.CompareNetObjects;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Tests.Services
{
    [Trait("Category", "LastBets")]
    [Collection("FanexDataCollection")]
    public class LastBetsServiceImplTests
    {
        private readonly LastBetsServiceImpl lastBetsService;
        private readonly IDynamicRepository dynamicRepository;
        private readonly CompareLogic compareLogic;

        public LastBetsServiceImplTests(FanexDataFixture dataFixture)
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LasteBetsVirtualSportRacingOutrightBetType"] = "1231,1232,1233",
                ["LastBetsOtherOutrightBetType"] = "10,31,32,33",
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

            lastBetsService = new LastBetsServiceImpl(dataFixture.DynamicRepository);

            dynamicRepository = dataFixture.DynamicRepository;
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetLastBets_Always_ReturnsCorrectData()
        {
            // Arrange
            var param = new GetLastBetsParam(0, 1, 0, 10);
            var expectedTickets = new List<TicketDto>
            {
                CreateTicketDto(BetTypeIdConstants.Outright, 0, null, null, null),
                CreateTicketDto(BetTypeIdConstants.Handicap, 0, null, null, null),
                CreateTicketDto(BetTypeIdConstants.VirtualHorseRacing_Win, 0, null, null, null),
                CreateTicketDto(BetTypeIdConstants.HorseRacing_Win, 1000, string.Empty, string.Empty, string.Empty),
                CreateTicketDto(BetTypeIdConstants.HorseRacing_Win, 1001, null, null, null),
                CreateTicketDto(BetTypeIdConstants.Handicap, 1002, null, null, null)
            };

            // Act
            var actualTickets = lastBetsService.GetLastBets(param).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(expectedTickets, actualTickets).AreEqual);
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
        public void IsEnableLastBetsMonitoring_NotInternalAndEnableConfig_ReturnTrue()
        {
            // Arrange
            var param = new CheckCanViewLastBetsParam(0, false, true, Enumerable.Empty<int>());

            // Act
            var actualCanViewLastBets = lastBetsService.CanViewLastBets(param);

            // Assert
            Assert.True(actualCanViewLastBets);
        }

        [Fact]
        public void IsEnableLastBetsMonitoring_CustIdInAlwaysAllowAccessList_ReturnTrue()
        {
            // Arrange
            var param = new CheckCanViewLastBetsParam(2, false, false, new List<int> { 1, 2, 3 });

            // Act
            var actualCanViewLastBets = lastBetsService.CanViewLastBets(param);

            // Assert
            Assert.True(actualCanViewLastBets);
        }

        [Fact]
        public void IsEnableLastBetsMonitoring_CustIdNotInAlwaysAllowAccessList_ReturnFalse()
        {
            // Arrange
            var param = new CheckCanViewLastBetsParam(4, false, false, new List<int> { 1, 2, 3 });

            // Act
            var actualCanViewLastBets = lastBetsService.CanViewLastBets(param);

            // Assert
            Assert.False(actualCanViewLastBets);
        }

        private TicketDto CreateTicketDto(
            int betType,
            int matchId,
            string race,
            string matchCode,
            string showTime)
            => new TicketDto
            {
                BetTypeId = betType,
                MatchId = matchId,
                Race = race,
                MatchCode = matchCode,
                ShowTime = showTime
            };
    }
}