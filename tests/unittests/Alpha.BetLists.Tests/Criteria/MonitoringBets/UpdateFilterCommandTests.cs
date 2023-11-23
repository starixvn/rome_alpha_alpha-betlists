namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Xunit;

    [Trait("Category", "UpdateFilter")]
    public class UpdateFilterCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new UpdateFilterCommand(1, 1, 1, "Default", true, "EN", "1.2.3.4", false);

            var expectedSettingKey = "Age_CustLastBetFilter_Upd";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidSportParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new UpdateFilterCommand(1, 1, 1, "Default", true, "EN", "1.2.3.4", false);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(0, 0, 0)]
        [InlineData(0, -1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, -1, 0)]
        [InlineData(0, 0, 0)]
        public void IsValid_InvalidParam_ReturnsFalse(int custId, int subAccId, int filterId)
        {
            // Arrange
            var criteria = new UpdateFilterCommand(custId, subAccId, filterId, "", true, "EN", "1.2.3.4", false);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}