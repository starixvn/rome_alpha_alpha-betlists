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
            var criteria = new GetEnableJackpotProductCriteria(0, false);
            var expectedSettingKey = "Acc_GameEnableJackpotSel";

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
            var criteria = new GetEnableJackpotProductCriteria(custId, false);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetEnableJackpotProductCriteria(1, false);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
}