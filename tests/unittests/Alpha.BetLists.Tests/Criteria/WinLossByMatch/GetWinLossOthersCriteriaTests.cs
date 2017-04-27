using Alpha.BetLists.Criteria.WinLossByMatch;
using Alpha.BetLists.Models;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.WinLossByMatch
{
    [Trait("Category", "WinLossByMatch")]
    public class GetWinLossOthersCriteriaTests
    {
        [Theory]
        [InlineData(4, "Age_BetList_SelWinlossByBettypeMatch_SAg")]
        [InlineData(3, "Age_BetList_SelWinlossByBettypeMatch_MAg")]
        [InlineData(2, "Age_BetList_SelWinlossByBettypeMatch_Ag")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var criteria = new GetWinLossOthersCriteria(0, 0, 0, string.Empty, string.Empty, false, Enumeration.FromValue<UserLevel>(custLevelId));

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
            var criteria = new GetWinLossOthersCriteria(custId, 0, 0, string.Empty, string.Empty, false, UserLevel.Agent);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }
    }
}
