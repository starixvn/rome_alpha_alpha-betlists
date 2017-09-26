namespace Alpha.BetLists.Tests.Criteria.LastBets
{
    using BetLists.Criteria.LastBets;
    using Collections;
    using Xunit;

    [Trait("Category", "LastBets")]
    [Collection("AnyValueCreatorCollection")]
    public class GetRunningOtherMatchCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetRunningOtherMatchCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetRunningOtherMatchCriteria();
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
            var criteria = new GetRunningOtherMatchCriteria();

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void MatchId_Always_ReturnsZero()
        {
            // Arrange
            var criteria = new GetRunningOtherMatchCriteria();
            var expectedMatchId = 0;

            // Act
            var actualMatchId = criteria.MatchId;

            // Assert
            Assert.Equal(expectedMatchId, actualMatchId);
        }
    }
}
