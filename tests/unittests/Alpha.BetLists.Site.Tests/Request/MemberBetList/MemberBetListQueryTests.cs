namespace Alpha.BetLists.Site.Tests.Request.MemberBetList
{
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using Fixtures;
    using Requests.MemberBetList;
    using System;
    using Utils;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    [Collection("BetListCollection")]
    public class MemberBetListQueryTests
    {
        private readonly IUser user;

        public MemberBetListQueryTests(BetListFixture betlistFixture)
        {
            user = betlistFixture.User;

            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void MinDate_Always_ReturnsFirstDayOfThreeMonthsAgo()
        {
            // Arrange
            var query = new MemberBetListQuery(user);
            var expectedMinDate = new DateTime(2017, 2, 1);

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(expectedMinDate, actualMinDate);
        }

        [Fact]
        public void FromDate_Always_ReturnsCurrentDate()
        {
            // Arrange
            var query = new MemberBetListQuery(user);
            var expectedFromDate = DefaultVariables.CurrentDate;

            // Act
            var actualFromDate = query.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnsNextSevenDay()
        {
            // Arrange
            var query = new MemberBetListQuery(user);
            var expectedToDate = new DateTime(2017, 6, 1);

            // Act
            var actualToDate = query.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}