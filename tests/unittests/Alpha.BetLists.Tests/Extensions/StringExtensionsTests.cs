using Alpha.BetLists.Extensions;
using Xunit;

namespace Alpha.BetLists.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void SplitByCommaWithNoneEmptyEntries_Always_ReturnsCorrectValue()
        {
            // Arrange
            var value = "1,2,,4,,,5";
            var expectedValue = new[] { "1", "2", "4", "5" };

            // Act
            var actualValue = value.SplitByCommaWithNoneEmptyEntries();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void ToListInt_Always_ReturnsCorrectValue()
        {
            // Arrange
            var value = new[] { "1", "2", "4", "5" };
            var expectedValue = new[] { 1, 2, 4, 5 };

            // Act
            var actualValue = value.ToListInt();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
