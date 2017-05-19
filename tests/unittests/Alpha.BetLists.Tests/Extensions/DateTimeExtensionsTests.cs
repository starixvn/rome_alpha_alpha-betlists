namespace Alpha.BetLists.Tests.Extensions
{
    using BetLists.Extensions;
    using System;
    using Xunit;

    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(12, 0, 0)]
        [InlineData(13, 0, 0)]
        [InlineData(23, 59, 59)]
        public void ToShortDayWithSlash_Always_ReturnsShortDayWithSlash(int hour, int minute, int second)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var expectedDay = "11/24/2016";

            // Act
            var actualDay = dateTime.ToShortDateWithSlash();

            // Assert
            Assert.Equal(expectedDay, actualDay);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(31, 1)]
        [InlineData(1, 6)]
        [InlineData(2, 6)]
        [InlineData(30, 6)]
        [InlineData(1, 12)]
        [InlineData(2, 12)]
        [InlineData(31, 12)]
        public void FirstDayOfMonth_Always_ReturnsFirstDayOfMonth(int day, int month)
        {
            // Arrange
            var dateTime = new DateTime(2016, month, day);

            // Act
            var actualDate = dateTime.FirstDayOfMonth();

            // Assert
            Assert.Equal(month, actualDate.Month);
            Assert.Equal(1, actualDate.Day);
        }
    }
}