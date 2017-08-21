namespace Alpha.BetLists.Tests.Criteria
{
    using BetLists.Criteria;
    using Xunit;

    public class GetBalanceUpDayAndMoveBetsDayCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetBalanceUpDayAndMoveBetsDayCriteria();
            var expectedSettingKey = "Age_SelBalanceUpDay";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetBalanceUpDayAndMoveBetsDayCriteria();

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
