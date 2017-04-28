using System;
using Alpha.BetLists.Criteria.WinLossByMatch;
using Alpha.BetLists.Models;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.WinLossByMatch
{
    [Trait("Category", "WinLossByMatch")]
    public class GetWinLossRacingCriteriaTests
    {
        [Theory]
        [InlineData(4, "Age_BetList_SelWinlossHR_SAg")]
        [InlineData(3, "Age_BetList_SelWinlossHR_MAg")]
        [InlineData(2, "Age_BetList_SelWinlossHR_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetWinLossRacingCriteria(0, 0, 0, string.Empty, string.Empty, DateTime.Today, DateTime.Today, DateTime.Today, custLevel);

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
            var criteria = new GetWinLossRacingCriteria(custId, 0, 0, string.Empty, string.Empty, DateTime.Today, DateTime.Today, DateTime.Today, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetWinLossRacingCriteria(1, 0, 0, string.Empty, string.Empty, DateTime.Today, DateTime.Today.AddDays(-1), DateTime.Today, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidCriteria_ReturnsTrue(int custId)
        {
            // Arrange
            var criteria = new GetWinLossRacingCriteria(custId, 0, 0, string.Empty, string.Empty, DateTime.Today, DateTime.Today, DateTime.Today, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
