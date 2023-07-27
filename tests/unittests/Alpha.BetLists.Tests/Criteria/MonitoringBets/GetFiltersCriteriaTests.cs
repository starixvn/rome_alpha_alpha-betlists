namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Xunit;

    [Trait("Category", "GetFilters")]
    public class GetFiltersCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetFiltersCriteria(1, 0);

            var expectedSettingKey = "Age_CustLastBetFilter_Sel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidSportParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetFiltersCriteria(1, 0);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, -1)]
        public void IsValid_InvalidParam_ReturnsFalse(int custId, int subAccId)
        {
            // Arrange
            var criteria = new GetFiltersCriteria(custId, subAccId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}