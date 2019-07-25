namespace Alpha.BetLists.Tests.Criteria.MemberBetList
{
    using System;
    using BetLists.Criteria.MemberBetList;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    public class GetBetListAllByDateCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetBetListAllByDateCriteria(0, string.Empty, DateTime.Today, DateTime.Today, string.Empty);
            var expectedSettingKey = "Age_BetList_SelAllByDate_Mem";

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
            var criteria = new GetBetListAllByDateCriteria(custId, string.Empty, DateTime.Today, DateTime.Today, string.Empty);

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
            var criteria = new GetBetListAllByDateCriteria(1, productIds, DateTime.Today, DateTime.Today, string.Empty);

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
            var criteria = new GetBetListAllByDateCriteria(1, productIds, DateTime.Today, DateTime.Today.AddDays(-1), string.Empty);

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
            var criteria = new GetBetListAllByDateCriteria(1, productIds, DateTime.Today, DateTime.Today, string.Empty);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}