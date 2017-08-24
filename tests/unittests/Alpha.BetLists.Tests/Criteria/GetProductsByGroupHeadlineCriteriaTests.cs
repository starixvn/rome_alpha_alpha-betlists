using Alpha.BetLists.Criteria;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria
{
    public class GetProductsByGroupHeadlineCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetProductsByGroupHeadlineCriteria(custId: 0);
            var expectedSettingKey = "Age_Rpt_ConsolidatedGroupProductSel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetProductsByGroupHeadlineCriteria(custId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidCustId_ReturnsTrue(int custId)
        {
            // Arrange
            var criteria = new GetProductsByGroupHeadlineCriteria(custId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
