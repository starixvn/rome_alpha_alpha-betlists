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
            var getProductsCriteria = new GetOfferedProductsBySiteCriteria();
            var expectedSettingKey = "Acc_CustSetting_CheckPGSoftEnable";

            // Act
            var actualSettingKey = getProductsCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_EmptySIte_ReturnsFalse()
        {
            // Arrange
            var getProductsCriteria = new GetOfferedProductsBySiteCriteria();

            // Act
            var actualIsValid = getProductsCriteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_SiteHasValue_ReturnsTrue()
        {
            // Arrange
            var getProductsCriteria = new GetOfferedProductsBySiteCriteria { Site = "IBCIndo" };

            // Act
            var actualIsValid = getProductsCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}