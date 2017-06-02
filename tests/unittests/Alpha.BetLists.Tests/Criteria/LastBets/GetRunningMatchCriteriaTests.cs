using Alpha.BetLists.Criteria.LastBets;
using Alpha.BetLists.Tests.Collections;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.LastBets
{
    [Trait("Category", "LastBets")]
    [Collection("AnyValueCreatorCollection")]
    public class GetRunningMatchCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetRunningMatchCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetRunningOtherMatchCriteria(matchId: anyValueCreator.Create<int>());
            var expectedSettingKey = "Age_Monitor_MatchSel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetRunningOtherMatchCriteria(matchId: anyValueCreator.Create<int>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
