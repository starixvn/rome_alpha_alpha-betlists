namespace Alpha.BetLists.Tests.Criteria.LastBets
{
    using BetLists.Criteria.LastBets;
    using Collections;
    using Xunit;

    [Trait("Category", "LastBets")]
    [Collection("AnyValueCreatorCollection")]
    public class GetMatchInfoByIdCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetMatchInfoByIdCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetMatchInfoByIdCriteria(5);
            var expectedSettingKey = "Age_Monitor_MatchSelByID";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
            Assert.Equal(5, criteria.MatchId);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetMatchInfoByIdCriteria(anyValueCreator.Create<int>());

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
