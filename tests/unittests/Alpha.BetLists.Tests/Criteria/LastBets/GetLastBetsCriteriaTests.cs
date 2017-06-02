using Alpha.BetLists.Criteria.LastBets;
using Alpha.BetLists.Tests.Collections;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.LastBets
{
    [Trait("Category", "LastBets")]
    [Collection("AnyValueCreatorCollection")]
    public class GetLastBetsCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetLastBetsCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Theory]
        [InlineData(0, "Age_Monitor_BetlistSel_Ag_AllBettypes")]
        [InlineData(1, "Age_Monitor_BetlistSel_Ag_HDP")]
        [InlineData(3, "Age_Monitor_BetlistSel_Ag_OU")]
        [InlineData(19001, "Age_Monitor_BetlistSel_Ag_OtherBettypes")]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey(
            int betType,
            string expectedSettingKey)
        {
            // Arrange
            var criteria = new GetLastBetsCriteria(
                betType,
                custId: anyValueCreator.Create<int>(),
                transId: anyValueCreator.Create<long>(),
                pageSize: anyValueCreator.Create<int>());

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue, false)]
        [InlineData(-1, false)]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_Always_ReturnsTrue(int custId, bool expectedIsValid)
        {
            // Arrange
            var criteria = new GetLastBetsCriteria(
                betType: anyValueCreator.Create<int>(),
                custId: custId,
                transId: anyValueCreator.Create<long>(),
                pageSize: anyValueCreator.Create<int>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }
    }
}
