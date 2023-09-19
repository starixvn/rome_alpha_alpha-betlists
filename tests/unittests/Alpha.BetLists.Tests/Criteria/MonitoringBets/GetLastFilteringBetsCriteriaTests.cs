namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Xunit;

    [Trait("Category", "GetLastFilteringBets")]
    public class GetLastFilteringBetsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetLastFilteringBetsCriteria(
                1, 1, "0", "0",
                "0", "0", 1, 10, 10);

            var expectedSettingKey = "Age_BetList_LastBets_Ag";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetLastFilteringBetsCriteria(
                1, 1, "0", "0",
                "0", "0", 1, 10, 10);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Theory]
        [InlineData("", 1, 1)]        
        [InlineData("0", -1, 1)]
        [InlineData("0", 1, 0)]
        public void IsValid_InvalidParam_ReturnsFalse(string betTypeList, int minStake, int limitRowCount)
        {
            // Arrange
            var criteria = new GetLastFilteringBetsCriteria(1, 1,
                "0", string.Empty, string.Empty, betTypeList,
            1, minStake, limitRowCount);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}