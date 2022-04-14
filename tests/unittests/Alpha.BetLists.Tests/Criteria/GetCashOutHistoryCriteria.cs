namespace Alpha.BetLists.Tests.Criteria
{
    using System;
    using BetLists.Criteria;
    using Xunit;

    public class GetCashOutHistoryCriteriaTest
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetCashOutHistoryCriteria(123, 321, DateTime.Now, DateTime.Now);
            var expectedSettingKey = "Acc_BetList_CashOut_History";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_CorrectPara_ReturnsTrue()
        {
            // Arrange
            var getProductsCriteria = new GetCashOutHistoryCriteria(123, 321, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = getProductsCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsValid_CustIdIsZero_ReturnsFalse()
        {
            // Arrange
            var getProductsCriteria = new GetCashOutHistoryCriteria(123, 0, DateTime.Now, DateTime.Now);

            // Act
            var act = getProductsCriteria.IsValid();

            // Assert
            Assert.False(act);
        }

        [Fact]
        public void IsValid_TransIdIsZero_ReturnsFalse()
        {
            // Arrange
            var getProductsCriteria = new GetCashOutHistoryCriteria(0, 10, DateTime.Now, DateTime.Now);

            // Act
            var act = getProductsCriteria.IsValid();

            // Assert
            Assert.False(act);
        }
    }
}