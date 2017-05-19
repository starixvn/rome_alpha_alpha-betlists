namespace Alpha.BetLists.Tests.Criteria
{
    using BetLists.Criteria;
    using Xunit;

    public class GetScoreMapBetTypesCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getProductsCriteria = new GetScoreMapBetTypesCriteria();
            var expectedSettingKey = "Age_ScoreMap_SelBettype";

            // Act
            var actualSettingKey = getProductsCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var getProductsCriteria = new GetScoreMapBetTypesCriteria();

            // Act
            var actualIsValid = getProductsCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}