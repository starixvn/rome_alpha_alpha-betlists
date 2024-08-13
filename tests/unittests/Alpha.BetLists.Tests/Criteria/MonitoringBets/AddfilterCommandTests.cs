namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Xunit;

    [Trait("Category", "AddFilter")]
    public class AddFilterCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new AddFilterCommand(
                    1,
                    1,
                    "Test",
                    true,
                    "0",
                    10,
                    "<root></root>",
                    1,
                    1,
                    "0",
                    "0",
                    "EN",
                    "1.2.3.4",
                    false);

            var expectedSettingKey = "Age_CustLastBetFilter_Ins";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new AddFilterCommand(
                    1,
                    1,
                    "Default",
                    true,
                    "0",
                    0,
                    "<root></root>",
                    1,
                    1,
                    "0",
                    "0",
                    "EN",
                    "1.2.3.4",
                    false);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Theory]
        [InlineData(0, 1, "Default", "0", 10)]
        [InlineData(1, -1, "Default", "0", 10)]
        [InlineData(1, 1, "", "0", 10)]
        [InlineData(1, 1, "Default", "", 10)]
        [InlineData(1, 1, "Default", "0", -1)]
        public void IsValid_InvalidParam_ReturnsFalse(int custId, int subAccId, string filterName, string betType, int stake)
        {
            // Arrange
            var criteria = new AddFilterCommand(
                    custId,
                    subAccId,
                    filterName,
                    true,
                    betType,
                    stake,
                    "<root></root>",
                    1,
                    1,
                    "0",
                    "0",
                    "EN",
                    "1.2.3.4",
                    false);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}