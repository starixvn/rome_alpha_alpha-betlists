namespace Alpha.BetLists.Tests.Extensions
{
    using BetLists.Extensions;
    using Xunit;

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("      ")]
        public void ToIntDictionary_NullOrWhiteSpace_ReturnsEmptyDictionary(string value)
        {
            // Arrange & Act
            var actualValue = value.ToIntDictionary();

            // Assert
            Assert.Equal(0, actualValue.Count);
        }

        [Theory]
        [InlineData("1,2,3,4,5")]
        [InlineData("1,2,3,4,5,1,2,3,4,5")]
        public void ToIntDictionary_ValidInputParam_ReturnsCorrectDictionary(string value)
        {
            // Arrange & Act
            var actualValue = value.ToIntDictionary();

            // Assert
            Assert.True(actualValue.Count > 0);
        }
    }
}
