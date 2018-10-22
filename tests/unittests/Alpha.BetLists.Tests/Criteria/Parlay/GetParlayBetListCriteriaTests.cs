namespace Alpha.BetLists.Tests.Criteria.Parlay
{
    using BetLists.Criteria.Parlay;
    using Collections;
    using Xunit;

    [Trait("Category", "Parlay")]
    [Collection("AnyValueCreatorCollection")]
    public class GetParlayBetListCriteriaTests
    {
        private readonly GetParlayBetListCriteria criteria;

        public GetParlayBetListCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            criteria = new GetParlayBetListCriteria(
                transId: anyValueCreator.Create<long>(),
                custId: anyValueCreator.Create<int>());
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_BetList_SelParlayDetail";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
