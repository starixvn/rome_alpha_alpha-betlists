namespace Alpha.BetLists.Tests.Criteria.Outstanding
{
    using BetLists.Criteria.Outstanding;
    using System;
    using Xunit;

    [Trait("Category", "Outstanding")]
    public class GetBetListRunningByDateCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetBetListRunningByDateCriteria(0, string.Empty, DateTime.Today, DateTime.Today, true, string.Empty, string.Empty);
            var expectedSettingKey = "Age_BetList_SelRunningByDate_Mem";

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
            var criteria = new GetBetListRunningByDateCriteria(custId, string.Empty, DateTime.Today, DateTime.Today, true, string.Empty, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void IsValid_ProductIdsIsNullOrWhiteSpace_ReturnsFalse(string productIds)
        {
            // Arrange
            var criteria = new GetBetListRunningByDateCriteria(1, productIds, DateTime.Today, DateTime.Today, true, string.Empty, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_InvalidDateRange_ReturnsFalse()
        {
            // Arrange
            var productIds = "1,2";
            var criteria = new GetBetListRunningByDateCriteria(1, productIds, DateTime.Today, DateTime.Today.AddDays(-1), true, string.Empty, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var productIds = "1,2";
            var criteria = new GetBetListRunningByDateCriteria(1, productIds, DateTime.Today, DateTime.Today, true, string.Empty, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}