namespace Alpha.BetLists.Tests.Criteria.MoveStatementHistory
{
    using Alpha.BetLists.Criteria.MoveStatementHistory;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    public class GetBetListMoveStatementHistoryCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_BetList_SelMovingEventByTransId";
            var criteria = new GetBetListMoveStatementHistoryCriteria(10, 123456);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
            Assert.Equal(10, criteria.CustId);
            Assert.Equal(123456, criteria.TransId);
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(-1, -1, false)]
        [InlineData(-1, 1, false)]
        [InlineData(1, -1, false)]
        [InlineData(int.MinValue, int.MinValue, false)]
        [InlineData(1, 1, true)]
        [InlineData(int.MaxValue, int.MaxValue, true)]
        public void IsValid_Always_ReturnsCorrectIsValid(int custId, int transId, bool expectedIsValid)
        {
            // Arrange
            var criteria = new GetBetListMoveStatementHistoryCriteria(custId, transId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }
    }
}
