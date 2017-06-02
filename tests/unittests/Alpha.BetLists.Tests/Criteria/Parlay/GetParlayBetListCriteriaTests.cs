using Alpha.BetLists.Criteria.Parlay;
using Alpha.BetLists.Tests.Collections;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.Parlay
{
    [Trait("Category", "Parlay")]
    [Collection("AnyValueCreatorCollection")]
    public class GetParlayBetListCriteriaTests
    {
        private readonly GetParlayBetListCriteria criteria;

        public GetParlayBetListCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            criteria = new GetParlayBetListCriteria(
                transId: anyValueCreator.Create<long>(),
                language: anyValueCreator.Create<string>());
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
