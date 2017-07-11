using Alpha.BetLists.Criteria.LastBets;
using Alpha.BetLists.Tests.Collections;
using Xunit;

namespace Alpha.BetLists.Tests.Criteria.LastBets
{
    [Trait("Category", "LastBets")]
    [Collection("AnyValueCreatorCollection")]
    public class GetRunningOutrightMatchCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreator;

        public GetRunningOutrightMatchCriteriaTests(AnyValueCreatorFixture anyValueCreator)
        {
            this.anyValueCreator = anyValueCreator;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetRunningOutrightMatchCriteria();
            var expectedSettingKey = "Age_Monitor_OutrightSel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetRunningOutrightMatchCriteria();

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsVirtualSportsRacing_Always_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetRunningOutrightMatchCriteria();
            var expectedIsVirtualSportsRacing = false;

            // Act
            var actualIsVirtualSportsRacing = criteria.IsVirtualSportsRacing;

            // Assert
            Assert.Equal(expectedIsVirtualSportsRacing, actualIsVirtualSportsRacing);
        }
    }
}
