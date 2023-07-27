namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Xunit;

    [Trait("Category", "DeleteFilter")]
    public class DeleteFilterCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new DeleteFilterCommand(1, 0, 1, "1.2.3.4");

            var expectedSettingKey = "Age_CustLastBetFilter_Del";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new DeleteFilterCommand(1, 0, 1, "1.2.3.4");

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }


        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(0, 0, 0)]
        [InlineData(0, -1, 0)]
        [InlineData(1, -1, 1)]
        [InlineData(1, -1, 0)]
        [InlineData(0, 0, 0)]
        public void IsValid_InvalidParam_ReturnsFalse(int custId, int subAccId, int filterId)
        {
            // Arrange
            var criteria = new DeleteFilterCommand(custId, subAccId, filterId, "1.2.3.4");

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}