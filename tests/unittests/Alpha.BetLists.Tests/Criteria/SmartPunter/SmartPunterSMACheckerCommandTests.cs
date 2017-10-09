namespace Alpha.BetLists.Tests.Criteria.SmartPunter
{
    using Alpha.SmartPunter.Criteria;
    using BetLists.Criteria.SmartPunter;
    using Xunit;

    [Trait("Category", "SmartPunter")]
    public class SmartPunterSMACheckerCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var commandData = new SmartPunterSMAChecker(0);
            var criteria = new SmartPunterSMACheckerCommand(commandData);
            var expectedSettingKey = "Age_CustomerSelCheckSmartPunterSMA";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_CustIdIsNullAndUsernameIsNull_ReturnsFalse()
        {
            // Arrange
            var commandData = new SmartPunterSMAChecker(null, null);
            var criteria = new SmartPunterSMACheckerCommand(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_CustIdIsNotNull_ReturnsTrue()
        {
            // Arrange
            var commandData = new SmartPunterSMAChecker(1, null);
            var criteria = new SmartPunterSMACheckerCommand(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsValid_UsernameIsNotNull_ReturnsTrue()
        {
            // Arrange
            var commandData = new SmartPunterSMAChecker(null, string.Empty);
            var criteria = new SmartPunterSMACheckerCommand(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
