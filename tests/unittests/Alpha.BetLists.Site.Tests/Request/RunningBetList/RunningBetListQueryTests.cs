namespace Alpha.BetLists.Site.Tests.Request.RunningBetList
{
    using System;
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using Fixtures;
    using Requests.RunningBetList;
    using Utils;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    [Collection("BetListCollection")]
    public class RunningBetListQueryTests
    {
        private readonly IUser user;

        public RunningBetListQueryTests(BetListFixture betlistFixture)
        {
            user = betlistFixture.User;

            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void MinDate_Always_ReturnsFirstDayOfThreeMonthsAgo()
        {
            // Arrange
            var query = new RunningBetListQuery(user);
            var expectedMinDate = new DateTime(2017, 2, 1);

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(expectedMinDate, actualMinDate);
        }

        [Fact]
        public void FromDate_Always_ReturnsSevenDaysAgo()
        {
            // Arrange
            var query = new RunningBetListQuery(user);
            var expectedFromDate = new DateTime(2017, 5, 18);

            // Act
            var actualFromDate = query.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnsCurrentDay()
        {
            // Arrange
            var query = new RunningBetListQuery(user);
            var expectedToDate = DefaultVariables.CurrentDate;

            // Act
            var actualToDate = query.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}