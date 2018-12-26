namespace Alpha.BetLists.Tests.Criteria.WinLossByMatch
{
    using BetLists.Criteria.WinLossByMatch;
    using BetLists.Models;
    using Xunit;

    [Trait("Category", "WinLossByMatch")]
    public class GetWinLossByMatchCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_BetList_SelWinlossByBettypeMatch";
            var criteria = new GetWinLossByMatchCriteria(0, 0, 0, string.Empty, false, UserLevel.Super);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(1, true)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_Always_ReturnsCorrectIsValid(int custId, bool expectedIsValid)
        {
            // Arrange
            var criteria = new GetWinLossByMatchCriteria(custId, 0, 0, string.Empty, false, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }
    }
}
