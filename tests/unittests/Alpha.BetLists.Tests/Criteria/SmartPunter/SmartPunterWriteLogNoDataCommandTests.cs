namespace Alpha.BetLists.Tests.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;
    using BetLists.Criteria.SmartPunter;
    using Xunit;

    [Trait("Category", "SmartPunter")]
    public class SmartPunterWriteLogNoDataCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var commandData = new SmartPunterWriteLogNoData(0, 0, false, 0, string.Empty);
            var criteria = new SmartPunterWriteLogNoDataCommand(commandData);
            var expectedSettingKey = "NAP_UserLog_SmartPunterIns";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var commandData = new SmartPunterWriteLogNoData(custId, 0, false, 0, string.Empty);
            var criteria = new SmartPunterWriteLogNoDataCommand(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsFalse()
        {
            // Arrange
            var commandData = new SmartPunterWriteLogNoData(1, 0, false, 0, string.Empty);
            var criteria = new SmartPunterWriteLogNoDataCommand(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
