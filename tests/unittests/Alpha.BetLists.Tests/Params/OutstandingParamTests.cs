namespace Alpha.BetLists.Tests.Params
{
    using BetLists.Params;
    using System;
    using System.Linq;
    using Xunit;

    [Trait("Category", "Outstanding")]
    public class OutstandingParamTests
    {
        [Fact]
        public void Constructor_GetAllIsTrue_SetFromDateIsSevenDaysAgo()
        {
            // Arrange
            var param = new OutstandingParam(1, Enumerable.Empty<int>(), DateTime.MinValue, DateTime.MinValue, true, string.Empty, string.Empty);
            var expectedFromDate = DateTime.Today.AddDays(-7);

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void Constructor_GetAllIsTrue_SetToDateIsToday()
        {
            // Arrange
            var param = new OutstandingParam(1, Enumerable.Empty<int>(), DateTime.MinValue, DateTime.MinValue, true, string.Empty, string.Empty);
            var expectedToDate = DateTime.Today;

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void Constructor_GetAllIsFalseAndNotSupportFromDate_SetFromDateIsOneDayAgo()
        {
            // Arrange
            var param = new OutstandingParam(1, Enumerable.Empty<int>(), DateTime.MinValue, DateTime.MinValue, false, string.Empty, string.Empty);
            var expectedFromDate = DateTime.Today.AddDays(-1);

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void Constructor_GetAllIsFalseAndNotSupportToDate_SetFromDateIsOneDayAgo()
        {
            // Arrange
            var param = new OutstandingParam(1, Enumerable.Empty<int>(), DateTime.MinValue, DateTime.MinValue, false, string.Empty, string.Empty);
            var expectedToDate = DateTime.Today.AddDays(-1);

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void Constructor_GetAllIsFalseAndSupportFromDate_ReturnsSupportedDate()
        {
            // Arrange
            var param = new OutstandingParam(1, Enumerable.Empty<int>(), DateTime.Today, DateTime.Today, false, string.Empty, string.Empty);
            var expectedFromDate = DateTime.Today;

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void Constructor_GetAllIsFalseAndSupportToDate_ReturnsSupportedDate()
        {
            // Arrange
            var param = new OutstandingParam(1, Enumerable.Empty<int>(), DateTime.Today, DateTime.Today, false, string.Empty, string.Empty);
            var expectedToDate = DateTime.Today;

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}