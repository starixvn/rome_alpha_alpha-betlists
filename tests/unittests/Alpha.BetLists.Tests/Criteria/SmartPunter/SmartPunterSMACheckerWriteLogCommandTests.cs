namespace Alpha.BetLists.Tests.Criteria.SmartPunter
{
    using AlphaPlus.SmartPunter.Criteria;
    using BetLists.Criteria.SmartPunter;
    using Xunit;

    [Trait("Category", "SmartPunter")]
    public class SmartPunterSMACheckerWriteLogCommandTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SmartPunterSMACheckerWriteLogCommand(string.Empty, 0, false, 0, string.Empty);
            var expectedSettingKey = "Age_CustomerSelCheckListSmartPunter";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
