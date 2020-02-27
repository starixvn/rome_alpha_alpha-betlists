namespace Alpha.BetLists.Tests.Criteria.RiskAnalysisIPMatching
{
    using Alpha.BetLists.Criteria.RiskAnalysisIPMatching;
    using System;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    public class GetRiskAnalysisIPMatchingCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var moveBetsDay = new DateTime(2020, 02, 16);
            var criteria = new GetRiskAnalysisIPMatchingCriteria(0, "192.1.1.1", new DateTime(2020, 02, 14), moveBetsDay);
            var expectedSettingKey = "Age_BetList_IPMatching";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
            Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(int.MinValue, false)]
        public void IsValid_CustIdProperty_ReturnsCorrectResult(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetRiskAnalysisIPMatchingCriteria(custId, "192.1.1.1", new DateTime(2020, 02, 14), new DateTime(2020, 02, 16));

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(actualIsValid, expectedResult);
        }

        [Theory]
        [InlineData("19.168.1.1", true)]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("  ", false)]
        public void IsValid_IPProperty_ReturnsCorrectResult(string ip, bool expectedResult)
        {
            // Arrange
            var criteria = new GetRiskAnalysisIPMatchingCriteria(1, ip, new DateTime(2020, 02, 14), new DateTime(2020, 02, 16));

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(actualIsValid, expectedResult);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetRiskAnalysisIPMatchingCriteria(1, "192.168.1.1", DateTime.MinValue, new DateTime(2020, 02, 16));

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetRiskAnalysisIPMatchingCriteria(1, "192.168.1.1", new DateTime(2020, 02, 14), new DateTime(2020, 02, 16));

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}