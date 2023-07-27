namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Xunit;

    [Trait("Category", "CheckUsernames")]
    public class CheckUsernamesCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new CheckUsernamesCriteria(1, "member01");

            var expectedSettingKey = "Age_CustLastBetFilter_Check";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidSportParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new CheckUsernamesCriteria(1, "member01");

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Theory]
        [InlineData(0, "member1")]
        [InlineData(1, "")]
        [InlineData(0, "")]
        public void IsValid_InvalidParam_ReturnsFalse(int agentId, string usernames)
        {
            // Arrange
            var criteria = new CheckUsernamesCriteria(agentId, usernames);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}