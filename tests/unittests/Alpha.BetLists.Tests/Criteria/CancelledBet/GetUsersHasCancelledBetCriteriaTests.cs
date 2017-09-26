namespace Alpha.BetLists.Tests.Criteria.CancelledBet
{
    using Alpha.BetLists.Criteria.CancelledBet;
    using Alpha.BetLists.Models;
    using Xunit;

    [Trait("Category", "CancelledBet")]
    public class GetUsersHasCancelledBetCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Agent_ReturnCorrectKey()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(263, UserLevel.Agent);
            var expectedKey = "Age_BetList_SelDeleted_UserList_Ag";
            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }

        [Fact]
        public void IsValid_Agent_CorrectAgentId_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(263, UserLevel.Agent);
            
            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_Agent_InCorrectAgentId_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(0, UserLevel.Agent);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }

        [Fact]
        public void GetSettingKey_Master_ReturnCorrectKey()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(263, UserLevel.Master);
            var expectedKey = "Age_BetList_SelDeleted_UserList_MAg";
            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }

        [Fact]
        public void IsValid_Master_CorrectMasterId_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(263, UserLevel.Master);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_Master_InCorrectMasterId_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(-263, UserLevel.Master);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }

        [Fact]
        public void GetSettingKey_Super_ReturnCorrectKey()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(263, UserLevel.Super);
            var expectedKey = "Age_BetList_SelDeleted_UserList_SAg";
            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }

        [Fact]
        public void IsValid_Super_CorrectSuperId_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(263, UserLevel.Super);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_Super_InCorrectSuperId_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetUsersHasCancelledBetCriteria(0, UserLevel.Super);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }
    }
}
