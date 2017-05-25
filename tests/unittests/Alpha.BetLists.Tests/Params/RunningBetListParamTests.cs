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
        public void FromDate_Always_ReturnStartOfFromDate()
        {
            // Arrange
            var fromDate = new DateTime(2017, 5, 25);
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), fromDate, DateTime.MinValue, false, string.Empty, string.Empty);
            var expectedFromDate = new DateTime(2017, 5, 25, 0, 0, 0);

            // Act
            var actualFromDate = param.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnEndOfToDate()
        {
            // Arrange
            var toDate = new DateTime(2017, 5, 25);
            var param = new RunningBetListParam(1, Enumerable.Empty<int>(), DateTime.MinValue, toDate, false, string.Empty, string.Empty);
            var expectedToDate = new DateTime(2017, 5, 25, 23, 59, 59);

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}