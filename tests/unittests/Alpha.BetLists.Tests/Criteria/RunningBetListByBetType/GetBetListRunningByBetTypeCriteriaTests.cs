namespace Alpha.BetLists.Tests.Criteria.RunningBetListByBetType
{
    using BetLists.Criteria.RunningBetListByBetType;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "RunningBetListByBetType")]
    public class GetBetListRunningByBetTypeCriteriaTests
    {
        [Theory]
        [InlineData(4, "Age_BetList_SelRunningByBettype_SAg")]
        [InlineData(3, "Age_BetList_SelRunningByBettype_MAg")]
        [InlineData(2, "Age_BetList_SelRunningByBettype_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetBetListRunningByBetTypeCriteria(0, 0, string.Empty, custLevel);

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
            var criteria = new GetBetListRunningByBetTypeCriteria(custId, 1, string.Empty, UserLevel.Agent);

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
            var criteria = new GetBetListRunningByBetTypeCriteria(1, betType, string.Empty, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetBetListRunningByBetTypeCriteria(1, 1, string.Empty, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
