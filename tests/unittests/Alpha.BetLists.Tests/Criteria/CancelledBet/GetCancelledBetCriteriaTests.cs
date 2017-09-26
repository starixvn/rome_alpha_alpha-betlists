namespace Alpha.BetLists.Tests.Criteria.CancelledBet
{
    using Alpha.BetLists.Criteria.CancelledBet;
    using Xunit;

    [Trait("Category", "CancelledBet")]
    public class GetCancelledBetCriteriaTests
    {
        [Fact]
        public void IsValid_ValidProperties_ReturnTrue()
        {
            //// Arrange
            var criteria = new GetCancelledBetCriteria(263, 2, "en");

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsValid_InValidProperties_ReturnFalse()
        {
            //// Arrange
            var criteria = new GetCancelledBetCriteria(-263, 2, "en");

            //// Act
            var act = criteria.IsValid();

            //// Assert
            Assert.False(act);
        }

        [Fact]
        public void GetSettingKey_Always_ReturnCorrectKey()
        {
            //// Arrange
            var criteria = new GetCancelledBetCriteria(263, 3, "en");
            var expectedKey = "Age_BetList_SelDeleted";

            //// Act
            var act = criteria.GetSettingKey();

            //// Assert
            Assert.Equal(expectedKey, act);
        }
    }
}
