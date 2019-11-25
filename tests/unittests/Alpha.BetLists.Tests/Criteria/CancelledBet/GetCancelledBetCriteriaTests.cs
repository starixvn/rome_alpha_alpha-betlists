using System;
using Alpha.BetLists.Criteria.CancelledBet;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.CancelledBet
{
    [Trait("Category", "GetCancelledBetCriteria")]
    public class GetCancelledBetCriteriaTests
    {
        [Fact]
        public void IsValid_ValidParameter_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetCancelledBetCriteria(1, DateTime.Now.AddDays(-1), DateTime.Now, DateTime.Now, 2, "1,2,3,4,5,6", "1,2,3", 50, 1);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_InvalidParameter_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetCancelledBetCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, 2, "", "", 50, 1);

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }

        [Fact]
        public void GetSettingKey_Always_Constant()
        {
            //// Arrange
            var criteria = new GetCancelledBetCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, 2, "", "", 50, 1);

            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal("Age_BetList_SelDeleted", act);
        }
    }
}
