namespace Alpha.BetLists.Tests.Criteria.SmartPunter
{
    using Alpha.SmartPunter.Criteria;
    using BetLists.Criteria.SmartPunter;
    using Xunit;

    [Trait("Category", "SmartPunter")]
    public class NAPUserPermissionCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var commandData = new NAPUserPermission(0);
            var criteria = new NAPUserPermissionCriteria(commandData);
            var expectedSettingKey = "Acc_Con_FileNameSelByCustID";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var commandData = new NAPUserPermission(custId);
            var criteria = new NAPUserPermissionCriteria(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var commandData = new NAPUserPermission(1);
            var criteria = new NAPUserPermissionCriteria(commandData);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
