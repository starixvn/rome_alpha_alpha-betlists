namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Xunit;

    [Trait("Category", "AddFilter")]
    public class AddFilterCommandTests
    {
        private static readonly SportXml[] Sports = { new SportXml() { League = "A", Sport = "Soccer" } };

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new AddFilterCommand(
                new AddFilterParam(1, 1,
                    "Test", true, "0", 10,
                    Sports, 1, "0", "0", 1,
                    "EN", "1.2.3.4"));

            var expectedSettingKey = "Age_CustLastBetFilter_Ins";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new AddFilterCommand(
                new AddFilterParam(1, 1,
                    "Default", true, "0", 0,
                    Sports, 1, "0", "0", 1,
                    "EN", "1.2.3.4"));

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }


        [Theory]
        [InlineData(0, 1, "Default", "0", 10)]
        [InlineData(1, -1, "Default", "0", 10)]
        [InlineData(1, 1, "", "0", 10)]
        [InlineData(1, 1, "Default", "", 10)]
        [InlineData(1, 1, "Default", "0", -1)]
        public void IsValid_InvalidParam_ReturnsFalse(int custId, int subAccId, string filterName, string betType, int stake)
        {
            // Arrange
            var criteria = new AddFilterCommand(
                new AddFilterParam(custId, subAccId,
                    filterName, true, betType, stake,
                    Sports, 1, "0", "0", 1,
                    "EN", "1.2.3.4"));

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}