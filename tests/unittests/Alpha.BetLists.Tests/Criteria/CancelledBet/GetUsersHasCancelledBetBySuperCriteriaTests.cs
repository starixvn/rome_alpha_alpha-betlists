using Alpha.BetLists.Criteria.CancelledBet;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.CancelledBet
{
    public class GetUsersHasCancelledBetBySuperCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetBySuperCriteria(263);
            var expectedKey = "Age_BetList_SelDeleted_UserList_SAg";
            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }

        [Fact]
        public void IsValid_CorrectSuperId_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetBySuperCriteria(263);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_InCorrectSuperId_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetBySuperCriteria(0);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }
    }
}
