namespace Alpha.BetLists.Tests.Criteria
{
    using BetLists.Criteria;
    using Xunit;

    public class GetOfferedProductsBySiteCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getProductsCriteria = new GetOfferedProductsBySiteCriteria(1, string.Empty, 1);
            var expectedSettingKey = "Acc_CustSetting_CheckMacauEnable";

            // Act
            var actualSettingKey = getProductsCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_EmptySIte_ReturnsFalse()
        {
            // Arrange
            var getProductsCriteria = new GetOfferedProductsBySiteCriteria(1, string.Empty, 1);

            // Act
            var actualIsValid = getProductsCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_SiteHasValue_ReturnsTrue()
        {
            // Arrange
            var getProductsCriteria = new GetOfferedProductsBySiteCriteria(1, "IBCIndo", 1);

            // Act
            var actualIsValid = getProductsCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}