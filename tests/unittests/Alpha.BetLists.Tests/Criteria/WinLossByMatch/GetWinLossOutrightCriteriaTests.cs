namespace Alpha.BetLists.Tests.Criteria.WinLossByMatch
{
    using System;
    using BetLists.Criteria.WinLossByMatch;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "WinLossByMatch")]
    public class GetWinLossOutrightCriteriaTests
    {
        [Theory]
        [InlineData(4, "Age_BetList_SelWinlossOutright_SAg")]
        [InlineData(3, "Age_BetList_SelWinlossOutright_MAg")]
        [InlineData(2, "Age_BetList_SelWinlossOutright_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new GetWinLossOutrightCriteria(0, 0, string.Empty, DateTime.Today, DateTime.Today, DateTime.Today, custLevel);

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
            var criteria = new GetWinLossOutrightCriteria(custId, 0, string.Empty, DateTime.Today, DateTime.Today, DateTime.Today, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetWinLossOutrightCriteria(1, 0, string.Empty, DateTime.Today, DateTime.Today.AddDays(-1), DateTime.Today, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidDateRange_ReturnsTrue(int custId)
        {
            // Arrange
            var criteria = new GetWinLossOutrightCriteria(custId, 0, string.Empty, DateTime.Today, DateTime.Today, DateTime.Today, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
