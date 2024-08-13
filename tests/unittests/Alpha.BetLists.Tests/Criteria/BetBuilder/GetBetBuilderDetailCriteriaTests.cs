namespace Alpha.BetLists.Tests.Criteria.BetBuilder
{
    using Alpha.BetLists.Criteria.BetBuilder;
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Xunit;

    [Trait("Category", "GetCancelledBetCriteria")]
    public class GetBetBuilderDetailCriteriaTests
    {
        [Fact]
        public void IsValid_ValidParameter_ReturnTrue()
        {
            // Arrange
            var param = new GetBetBuilderDetailParameter();
            var criteria = new GetBetBuilderDetailCriteria(0, 0);

            // Act
            var act = criteria.IsValid();

            // Assert
            Assert.Equal(act, param.IsValid());
        }

        [Fact]
        public void GetSettingKey_Always_Constant()
        {
            // Arrange
            var param = new GetBetBuilderDetailParameter();
            var criteria = new GetBetBuilderDetailCriteria(0, 0);

            // Act
            var act = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Acc_BetList_BetBuilder_Detail", act);
            Assert.Equal(param.CustomerId, criteria.CustId);
            Assert.Equal(param.TransactionId, criteria.TransId);
        }
    }
}
