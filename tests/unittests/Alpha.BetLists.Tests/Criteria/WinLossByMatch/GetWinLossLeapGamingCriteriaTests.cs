namespace Alpha.BetLists.Tests.Criteria.WinLossByMatch
{
    using System;
    using BetLists.Criteria.WinLossByMatch;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "WinLossByMatch")]
    public class GetWinLossLeapGamingCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_BetList_SelWinLossByBetType";
            var criteria = new GetWinLossByBetTypeCriteria(1, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Super.Value, 1);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsCorrectIsValid()
        {
            // Arrange
            var criteria = new GetWinLossByBetTypeCriteria(1, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Super.Value, 1);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsValid_CustIdIsInvalide_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetWinLossByBetTypeCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Super.Value, 1);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_FromToIsInvalide_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetWinLossByBetTypeCriteria(1, DateTime.Now.AddDays(1), DateTime.Now, DateTime.Now, UserLevel.Super.Value, 1);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}
