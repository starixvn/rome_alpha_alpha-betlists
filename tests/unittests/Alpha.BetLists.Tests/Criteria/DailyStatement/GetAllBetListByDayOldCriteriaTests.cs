using Alpha.BetLists.Criteria.DailyStatement;
using System;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.DailyStatement
{
    public class GetAllBetListByDayOldCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetAllBetListByDayOldCriteria(0, DateTime.Today, 0, string.Empty);
            var expectedSettingKey = "Age_BetList_SelByDayAll_Mem_WH";

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
            var criteria = new GetAllBetListByDayOldCriteria(custId, DateTime.Today, 0, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetAllBetListByDayOldCriteria(1, DateTime.Today, 0, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
