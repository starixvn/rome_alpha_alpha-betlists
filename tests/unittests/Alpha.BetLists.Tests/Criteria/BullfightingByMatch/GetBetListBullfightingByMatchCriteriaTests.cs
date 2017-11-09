namespace Alpha.BetLists.Tests.Criteria.BullfightingByMatch
{
    using BetLists.Criteria.BullfightingByMatch;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "BullfightingByMatch")]
    public class GetBetListBullfightingByMatchCriteriaTests
    {
        [Theory]
        [InlineData(4, "Age_BetList_TotalBets_BF_SAg")]
        [InlineData(3, "Age_BetList_TotalBets_BF_MAg")]
        [InlineData(2, "Age_BetList_TotalBets_BF_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetBetListBullfightingByMatchCriteria(0, 0, custLevel);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(int.MinValue, int.MinValue)]
        public void IsValid_InvalidParameter_ReturnsFalse(int custId, int matchId)
        {
            // Arrange
            var criteria = new GetBetListBullfightingByMatchCriteria(custId, matchId, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
