namespace Alpha.BetLists.Tests.Criteria.ParlayWinLossByMatch
{
    using System;
    using BetLists.Criteria.ParlayWinLossByMatch;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "ParlayWinLossByMatch")]
    public class GetParlayWinLossByMatchCriteriaTests
    {
        [Theory]
        [InlineData(4, "Age_BetList_SelWinlossParlay_SAg")]
        [InlineData(3, "Age_BetList_SelWinlossParlay_MAg")]
        [InlineData(2, "Age_BetList_SelWinlossParlay_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetParlayWinLossByMatchCriteria(0, 0, string.Empty, DateTime.Now, DateTime.Now, DateTime.Now, custLevel);

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
            var criteria = new GetParlayWinLossByMatchCriteria(custId, 1, string.Empty, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Agent);

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
            var criteria = new GetParlayWinLossByMatchCriteria(1, betType, string.Empty, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetParlayWinLossByMatchCriteria(1, 1, string.Empty, DateTime.Now, DateTime.Now.AddDays(-1), DateTime.Now, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetParlayWinLossByMatchCriteria(1, 1, string.Empty, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
