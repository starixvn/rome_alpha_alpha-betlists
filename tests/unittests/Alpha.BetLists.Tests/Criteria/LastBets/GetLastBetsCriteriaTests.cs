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
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
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
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_InvalidPageSize_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetLastBetsCriteria(
                betType: anyValueCreator.Create<int>(),
                custId: custId,
                transId: anyValueCreator.Create<long>(),
                pageSize: 0);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidCriteria_ReturnsTrue(int custId)
        {
            // Arrange
            var criteria = new GetLastBetsCriteria(
                betType: anyValueCreator.Create<int>(),
                custId: custId,
                transId: anyValueCreator.Create<long>(),
                pageSize: 1);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
