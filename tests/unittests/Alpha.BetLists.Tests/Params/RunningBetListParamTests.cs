namespace Alpha.BetLists.Tests.Params
{
    using BetLists.Params;
    using System;
    using System.Linq;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    public class RunningBetListParamTests
    {
        [Fact]
        public void Constructor_NotSupportFromDate_SetFromDateIsSevenDayAgo()
        {
            // Arrange
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.MinValue, DateTime.MinValue, false, string.Empty, string.Empty);
            var expectedFromDate = DateTime.Today.AddDays(-7);

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void Constructor_NotSupportToDate_SetFromDateIsToday()
        {
            // Arrange
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.MinValue, DateTime.MinValue, false, string.Empty, string.Empty);
            var expectedToDate = DateTime.Today;

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void Constructor_SupportFromDate_ReturnsSupportedDate()
        {
            // Arrange
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.Today, DateTime.Today, false, string.Empty, string.Empty);
            var expectedFromDate = DateTime.Today;

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void Constructor_SupportToDate_ReturnsSupportedDate()
        {
            // Arrange
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.Today, DateTime.Today, false, string.Empty, string.Empty);
            var expectedToDate = DateTime.Today;

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}