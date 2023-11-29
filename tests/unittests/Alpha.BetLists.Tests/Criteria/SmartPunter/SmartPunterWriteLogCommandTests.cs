using Alpha.BetLists.Criteria.SmartPunter;
using AlphaPlus.SmartPunter.Criteria;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.SmartPunter
{
    [Trait("Category", "SmartPunter")]
    public class SmartPunterWriteLogCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var commandData = new SmartPunterWriteLog(string.Empty, 0, false, 0, string.Empty);
            var criteria = new SmartPunterWriteLogCommand(commandData);
            var expectedSettingKey = "Age_UserLog_SmartPunterInsMultiple";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsValid_ListUserNameIsNullOrEmpty_ReturnsFalse(string listUserName)
        {
            // Arrange
            var commandData = new SmartPunterWriteLog(listUserName, 0, false, 0, string.Empty);
            var criteria = new SmartPunterWriteLogCommand(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }


        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var commandData = new SmartPunterWriteLog("username", 0, false, 0, string.Empty);
            var criteria = new SmartPunterWriteLogCommand(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
