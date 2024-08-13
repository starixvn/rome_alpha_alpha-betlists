namespace Alpha.BetLists.Tests.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;
    using BetLists.Criteria.SmartPunter;
    using Xunit;

    [Trait("Category", "SmartPunter")]
    public class SmartPunterSMACheckerCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SmartPunterSMACheckerCommand(0, null);
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
            var criteria = new SmartPunterSMACheckerCommand(null, null);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_CustIdIsNotNull_ReturnsTrue()
        {
            // Arrange
            var criteria = new SmartPunterSMACheckerCommand(1, null);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsValid_UsernameIsNotNull_ReturnsTrue()
        {
            // Arrange
            var criteria = new SmartPunterSMACheckerCommand(null, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
