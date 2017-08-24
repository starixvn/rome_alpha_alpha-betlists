using System;
using Alpha.BetLists.Criteria.Parlay;
using Alpha.BetLists.Tests.Collections;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.Parlay
{
    [Trait("Category", "Parlay")]
    [Collection("AnyValueCreatorCollection")]
    public class GetComboParlayDetailCriteriaTests
    {
        private readonly GetComboParlayDetailCriteria criteria;

        public GetComboParlayDetailCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            criteria = new GetComboParlayDetailCriteria(
                transId: anyValueCreator.Create<long>(),
                refNo: anyValueCreator.Create<long>(),
                betId: anyValueCreator.Create<long>(),
                custId: anyValueCreator.Create<int>(),
                balanceUpDate: anyValueCreator.Create<DateTime>(),
                moveBetDate: anyValueCreator.Create<DateTime>(),
                language: anyValueCreator.Create<string>());
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
