namespace Alpha.BetLists.Tests.Criteria.RunningBetListByBetTypeAndMatch
{
    using BetLists.Criteria.RunningBetListByBetTypeAndMatch;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "RunningByBetTypeAndMatch")]
    public class GetBetListRunningByBetTypeAndMatchCriteriaTests
    {
        [Theory]
        [InlineData(4, "Ost_BetList_TotalBet")]
        [InlineData(3, "Ost_BetList_TotalBet")]
        [InlineData(2, "Ost_BetList_TotalBet")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetBetListRunningByBetTypeAndMatchCriteria(0, 0, 0, 1, string.Empty, custLevel);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
            Assert.Equal(custLevelId, criteria.RoleId);
            Assert.Equal(0, criteria.BetType);
            Assert.Equal("", criteria.BetTeam);
            Assert.Equal(1, criteria.LiveIndicator);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetBetListRunningByBetTypeAndMatchCriteria(0, custId, 1, 0, string.Empty, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidBetType_ReturnsFalse(int betType)
        {
            // Arrange
            var criteria = new GetBetListRunningByBetTypeAndMatchCriteria(0, 1, betType, 0, string.Empty, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetBetListRunningByBetTypeAndMatchCriteria(0, 1, 1, 0, string.Empty, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
