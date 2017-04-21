namespace Alpha.BetLists.Tests.Services
{
    using System;
    using BetLists.Services;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fixtures;
    using Models;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexLibsCollection")]
    public class ReportDateServiceImplTests
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private readonly FanexCacheFixture cacheFixture;
        private readonly FanexDataFixture dataFixture;
        private readonly ReportDateService reportDateService;
        private BalanceUpDayAndMoveBetsDay dataInCache;
        private BalanceUpDayAndMoveBetsDay dataInDatabase;

        public ReportDateServiceImplTests(FanexCacheFixture cacheFixture, FanexDataFixture dataFixture)
        {
            this.cacheFixture = cacheFixture;

            // Init cache data
            dataInCache = new BalanceUpDayAndMoveBetsDay(new DateTime(2012, 1, 1), new DateTime(2012, 1, 1));
            dataInDatabase = new BalanceUpDayAndMoveBetsDay(new DateTime(2015, 2, 2), new DateTime(2015, 2, 2));

            dataFixture.DynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<CriteriaBase>()).Returns(dataInDatabase);
            this.dataFixture = dataFixture;

            reportDateService = new ReportDateServiceImpl(this.dataFixture.DynamicRepository);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasData_ReturnFromCache()
        {
            // Arrange
            BalanceUpDayAndMoveBetsDay expectedData;
            cacheFixture.CacheStorage.TryGet(BalanceUpDayAndMoveBetsDayCacheKey, out expectedData).Returns(x =>
            {
                x[1] = dataInCache;
                return dataInCache != null;
            });

            // Act
            var balanceUpDayAndMoveBetsDay = reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.Equal(dataInCache, balanceUpDayAndMoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasNoData_ReturnFromDatabase()
        {
            // Act
            var balanceUpDayAndMoveBetsDay = reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.Equal(dataInDatabase, balanceUpDayAndMoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasNoData_CallDynamicRepository()
        {
            // Act
            var balanceUpDayAndMoveBetsDay = reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            dataFixture.DynamicRepository.Received().Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>());
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_CacheHasNoData_SetDataToCache()
        {
            // Arrange
            dataInCache = null;

            // Act
            var balanceUpDayAndMoveBetsDay = reportDateService.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            cacheFixture.CacheStorage.Received().Set(
                Arg.Is(BalanceUpDayAndMoveBetsDayCacheKey),
                Arg.Is<BalanceUpDayAndMoveBetsDay>(data => data.BalanceUpDay == dataInDatabase.BalanceUpDay),
                Arg.Any<CacheItemOptions>());
        }

        [Fact]
        public void GetBalanceUpDay_Always_ReturnData()
        {
            // Act
            var balanceUpDay = reportDateService.GetBalanceUpDay();

            // Assert
            Assert.Equal(dataInDatabase.BalanceUpDay, balanceUpDay);
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnData()
        {
            // Act
            var moveBetsDay = reportDateService.GetMoveBetsDay();

            // Assert
            Assert.Equal(dataInDatabase.MoveBetsDay, moveBetsDay);
        }
    }
}