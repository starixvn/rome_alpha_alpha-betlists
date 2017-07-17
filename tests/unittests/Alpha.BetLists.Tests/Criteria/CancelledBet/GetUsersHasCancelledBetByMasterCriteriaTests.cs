using Alpha.BetLists.Criteria.CancelledBet;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.CancelledBet
{
    [Trait("Category", "CancelledBet")]
    public class GetUsersHasCancelledBetByMasterCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetByMasterCriteria(263);
            var expectedKey = "Age_BetList_SelDeleted_UserList_MAg";
            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }

        [Fact]
        public void IsValid_CorrectMasterId_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetByMasterCriteria(263);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_InCorrectMasterId_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetByMasterCriteria(-263);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }
    }
}
