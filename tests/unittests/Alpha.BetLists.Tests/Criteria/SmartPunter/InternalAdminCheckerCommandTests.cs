namespace Alpha.BetLists.Tests.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;
    using BetLists.Criteria.SmartPunter;
    using Xunit;

    [Trait("Category", "SmartPunter")]
    public class InternalAdminCheckerCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new InternalAdminCheckerCommand(0);
            var expectedSettingKey = "Acc_CustomerIsInternalAdmin";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new InternalAdminCheckerCommand(custId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new InternalAdminCheckerCommand(1);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
