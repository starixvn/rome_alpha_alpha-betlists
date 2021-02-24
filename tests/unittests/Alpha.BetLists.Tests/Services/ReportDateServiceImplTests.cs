namespace Alpha.BetLists.Tests.Services
{
    using BetLists.Criteria;
    using BetLists.Models;
    using BetLists.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using System;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexLibsCollection")]
    public class ReportDateServiceImplTests
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private readonly FanexDataFixture dataFixture;
        private readonly ReportDateService reportDateService;
        private BalanceUpDayAndMoveBetsDay dataInCache;
        private BalanceUpDayAndMoveBetsDay dataInDatabase;

        public ReportDateServiceImplTests(FanexDataFixture dataFixture)
        {
            // Init cache data
            dataInCache = new BalanceUpDayAndMoveBetsDay(new DateTime(2012, 1, 1), new DateTime(2012, 1, 1));
            dataInDatabase = new BalanceUpDayAndMoveBetsDay(new DateTime(2015, 2, 2), new DateTime(2015, 2, 2));

            dataFixture.DynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<CriteriaBase>()).Returns(dataInDatabase);
            this.dataFixture = dataFixture;

            reportDateService = new ReportDateServiceImpl(this.dataFixture.DynamicRepository, this.dataFixture.CacheService);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasData_ReturnFromCache()
        {
            // Arrange
            dataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>()).Returns(dataInCache);

            // Act
            var balanceUpDayAndMoveBetsDay = reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.Equal(dataInCache, balanceUpDayAndMoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasNoData_ReturnFromDatabase()
        {
            // Arrange
            dataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>()).Returns(dataInDatabase);

            // Act
            var balanceUpDayAndMoveBetsDay = reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.Equal(dataInDatabase, balanceUpDayAndMoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasNoData_CallDynamicRepository()
        {
            // Arrange
            dataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<BalanceUpDayAndMoveBetsDay>>(1);
                return func();
            });

            // Act
            reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            dataFixture.DynamicRepository.Received().Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>());
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasNoData_SetDataToCache()
        {
            // Act
            reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            dataFixture.CacheService.Received().GetOrSet(
                Arg.Is(BalanceUpDayAndMoveBetsDayCacheKey),
                Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(),
                Arg.Any<CacheItemOptions>());
        }

        [Fact]
        public void GetBalanceUpDay_Always_ReturnData()
        {
            // Arrange
            dataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<BalanceUpDayAndMoveBetsDay>>(1);
                return func();
            });

            // Act
            var balanceUpDay = reportDateService.GetBalanceUpDay();

            // Assert
            Assert.Equal(dataInDatabase.BalanceUpDay, balanceUpDay);
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnData()
        {
            // Arrange
            dataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<BalanceUpDayAndMoveBetsDay>>(1);
                return func();
            });

            // Act
            var moveBetsDay = reportDateService.GetMoveBetsDay();

            // Assert
            Assert.Equal(dataInDatabase.MoveBetsDay, moveBetsDay);
        }
    }
}