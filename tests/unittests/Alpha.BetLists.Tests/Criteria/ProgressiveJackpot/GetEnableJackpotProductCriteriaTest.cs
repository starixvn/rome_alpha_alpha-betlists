namespace Alpha.BetLists.Tests.Criteria.ProgressiveJackpot
{
    using BetLists.Criteria.ProgressiveJackpot;
    using Xunit;

    [Trait("Category", "ProgressiveJackpot")]
    public class GetEnableJackpotProductCriteriaTest
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetEnableJackpotProductCriteria();
            var expectedSettingKey = "Acc_GameLaunchSel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
        
        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetEnableJackpotProductCriteria();

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
}