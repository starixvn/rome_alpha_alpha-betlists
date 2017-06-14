using Alpha.BetLists.Criteria.CancelledBet;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.CancelledBet
{
    public class GetUsersHasCancelledBetByAgentCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetByAgentCriteria(263);
            var expectedKey = "Age_BetList_SelDeleted_UserList_Ag";
            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }

        [Fact]
        public void IsValid_CorrectAgentId_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetByAgentCriteria(263);
            
            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_InCorrectAgentId_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetByAgentCriteria(0);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }
    }
}
