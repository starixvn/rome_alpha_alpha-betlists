namespace Alpha.BetLists.Tests.Criteria.RacingByMatch
{
    using BetLists.Criteria.RacingByMatch;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "RacingByMatch")]
    public class GetBetListRacingByMatchCriteriaTests
    {
        [Theory]
        [InlineData(4, "Age_BetList_SelAllByDateHR_SAg")]
        [InlineData(3, "Age_BetList_SelAllByDateHR_MAg")]
        [InlineData(2, "Age_BetList_SelAllByDateHR_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetBetListRacingByMatchCriteria(0, 0, 0, string.Empty, true, custLevel);

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
            var criteria = new GetBetListRacingByMatchCriteria(custId, 1, 0, string.Empty, true, UserLevel.Agent);

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
            var criteria = new GetBetListRacingByMatchCriteria(1, betType, 0, string.Empty, true, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_IsShowRacingIsFalse_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetBetListRacingByMatchCriteria(1, 1, 0, string.Empty, false, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetBetListRacingByMatchCriteria(1, 1, 0, string.Empty, true, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
