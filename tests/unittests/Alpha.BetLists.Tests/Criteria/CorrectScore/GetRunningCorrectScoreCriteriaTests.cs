namespace Alpha.BetLists.Tests.Criteria.CorrectScore
{
    using BetLists.Criteria.CorrectScore;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "CorrectScore")]
    public class GetRunningCorrectScoreCriteriaTests
    {

        [Theory]
        [InlineData(4, "Age_TotalBets_Sel430_SAg")]
        [InlineData(3, "Age_TotalBets_Sel430_MAg")]
        [InlineData(2, "Age_TotalBets_Sel430_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetRunningCorrectScoreCriteria(0, string.Empty, custLevel);

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
            var criteria = new GetRunningCorrectScoreCriteria(custId, string.Empty, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }


        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetRunningCorrectScoreCriteria(1, string.Empty, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
