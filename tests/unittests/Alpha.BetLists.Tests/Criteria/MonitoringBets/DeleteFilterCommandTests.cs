namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Xunit;

    [Trait("Category", "DeleteFilter")]
    public class DeleteFilterCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new DeleteFilterParam(1, 0, 1, "EN", "1.2.3.4");
            var criteria = new DeleteFilterCommand(param);

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
            var param = new DeleteFilterParam(1, 0, 1, "EN", "1.2.3.4");
            var criteria = new DeleteFilterCommand(param);

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
            var param = new DeleteFilterParam(custId, custId, filterId, "EN", "1.2.3.4");
            var criteria = new DeleteFilterCommand(param);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}