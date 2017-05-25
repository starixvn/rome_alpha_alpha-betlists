namespace Alpha.BetLists.Tests.Utils
{
    using BetLists.Utils;
    using Constants;
    using Xunit;

    public class SystemTimeTests
    {
        [Fact]
        public void Now_Always_ReturnsValueSetToNowProperty()
        {
            // Arrange
            var expectedDate = DefaultVariables.CurrentDate;
            SystemTime.Today = expectedDate;

            // Act
            var actualDate = SystemTime.Today;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}