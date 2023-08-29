namespace Alpha.BetLists.Tests.Criteria.SearchTickets
{
    using System;
    using BetLists.Criteria.SearchTickets;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "SearchTickets")]
    public class SearchTicketsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new SearchTicketsCriteria(1, 1, UserLevel.Agent, DateTime.Now, DateTime.Now);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_BetList_SearchByRefNo", actualSettingKey);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        public void IsValid_InvalidCriteria_ReturnsFalse(long refNo, int uplineId)
        {
            // Arrange
            var criteria = new SearchTicketsCriteria(refNo, uplineId, UserLevel.Agent, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new SearchTicketsCriteria(1, 1, UserLevel.Agent, DateTime.Now, DateTime.Now);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
