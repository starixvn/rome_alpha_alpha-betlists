namespace Alpha.BetLists.Tests.Criteria.Parlay
{
    using System;
    using BetLists.Criteria.Parlay;
    using Collections;
    using Xunit;

    [Trait("Category", "Parlay")]
    [Collection("AnyValueCreatorCollection")]
    public class GetComboParlayDetailCriteriaTests
    {
        private readonly GetComboParlayDetailCriteria criteria;

        public GetComboParlayDetailCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            criteria = new GetComboParlayDetailCriteria(
                refNo: anyValueCreator.Create<long>(),
                custId: anyValueCreator.Create<int>(),
                balanceUpDate: anyValueCreator.Create<DateTime>(),
                moveBetDate: anyValueCreator.Create<DateTime>());
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Acc_BetlistCombinationParlay_Details";

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
