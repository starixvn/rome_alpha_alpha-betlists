namespace Alpha.BetLists.Tests.Models
{
    using BetLists.Models;
    using Xunit;

    public class ProductTests
    {
        [Fact]
        public void DisplayName_SetFromProductName_ReturnsDisplayNameFromCtor()
        {
            // Arrange
            var productDisplayName = "productDisplayName";
            var product = new Product(0, productDisplayName, null, null);

            // Act
            var actual = product.DisplayName;

            // Assert
            Assert.Equal(productDisplayName, actual);
        }

        [Fact]
        public void DisplayName_SetFunctionGetDisplayName_ReturnsDisplayNameFromCtor()
        {
            // Arrange
            var productDisplayName = "productDisplayName";
            var productId = 0;
            var product = new Product(productId, string.Empty, null, (id) => productDisplayName);

            // Act
            var actual = product.DisplayName;

            // Assert
            Assert.Equal(productDisplayName, actual);
        }
    }
}