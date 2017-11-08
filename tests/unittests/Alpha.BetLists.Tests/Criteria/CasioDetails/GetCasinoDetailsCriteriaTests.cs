using Alpha.BetLists.Criteria.CasinoDetails;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.CasinoDetails
{
    [Trait("Category", "CasinoDetails")]
    public class GetCasinoDetailsCriteriaTests
    {
        [Theory]
        [InlineData(6)]
        [InlineData(21)]
        public void GetSettingKey_ProductIdIsSupported_ReturnsCorrectKey(int productId)
        {
            // Arrange
            var criteria = new GetCasinoDetailsCriteria(1, 1, 1, productId, 1, 100);
            var expectedSettingKey = "Acc_BetList_Detail_" + productId;

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidRefNo_ReturnsFalse(int refNo)
        {
            // Arrange
            var criteria = new GetCasinoDetailsCriteria(refNo, 1, 1, 6, 1, 100);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetCasinoDetailsCriteria(1, custId, 1, 6, 1, 100);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidProductId_ReturnsFalse(int productId)
        {
            // Arrange
            var criteria = new GetCasinoDetailsCriteria(1, 1, 1, productId, 1, 100);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetCasinoDetailsCriteria(1, 1, 1, 6, 1, 100);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
