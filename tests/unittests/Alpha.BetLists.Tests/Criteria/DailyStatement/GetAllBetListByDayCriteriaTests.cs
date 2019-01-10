namespace Alpha.BetLists.Tests.Criteria.DailyStatement
{
    using System;
    using BetLists.Criteria.DailyStatement;
    using Xunit;

    [Trait("Category", "DailyStatement")]
    public class GetAllBetListByDayCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetAllBetListByDayCriteria(0, DateTime.Today, DateTime.Today, 0);
            var expectedSettingKey = "Age_BetList_SelByDayAll_Mem";

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
            var criteria = new GetAllBetListByDayCriteria(custId, DateTime.Today, DateTime.Today, 0);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetAllBetListByDayCriteria(1, DateTime.Today, DateTime.Today, 0);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
