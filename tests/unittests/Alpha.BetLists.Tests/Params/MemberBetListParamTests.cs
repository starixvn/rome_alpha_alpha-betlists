namespace Alpha.BetLists.Tests.Params
{
    using System;
    using System.Linq;
    using BetLists.Params;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    public class MemberBetListParamTests
    {
        [Fact]
        public void FromDate_Always_ReturnStartOfFromDate()
        {
            // Arrange
            var fromDate = new DateTime(2017, 5, 25);
            var param = new MemberBetListParam(1, Enumerable.Empty<int>(), fromDate, DateTime.MinValue, DateTime.MinValue, string.Empty, string.Empty);
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
            var param = new MemberBetListParam(1, Enumerable.Empty<int>(), DateTime.MinValue, toDate, DateTime.MinValue, string.Empty, string.Empty);
            var expectedToDate = new DateTime(2017, 5, 25, 23, 59, 59);

            // Act
            var actualToDate = param.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }
    }
}