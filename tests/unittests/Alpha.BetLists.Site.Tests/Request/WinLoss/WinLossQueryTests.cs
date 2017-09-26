namespace Alpha.BetLists.Site.Tests.Request.WinLoss
{
    using System;
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using Fixtures;
    using Requests.WinLoss;
    using Utils;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    [Collection("BetListCollection")]
    public class WinLossQueryTests
    {
        private readonly IUser user;

        public WinLossQueryTests(BetListFixture betlistFixture)
        {
            user = betlistFixture.User;

            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void MinDate_IsGroupHeadline_ReturnsFirstDayOfThreeMonthsAgo()
        {
            // Arrange
            var query = new WinLossQuery(user);
            query.IsGroupHeadline = true;

            var expectedMinDate = new DateTime(2017, 2, 1);

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(expectedMinDate, actualMinDate);
        }

        [Fact]
        public void MinDate_NotIsGroupHeadline_ReturnsMinDateTime()
        {
            // Arrange
            var query = new WinLossQuery(user);
            query.IsGroupHeadline = false;

            var expectedMinDate = DateTime.MinValue;

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(expectedMinDate, actualMinDate);
        }
    }
}
