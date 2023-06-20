namespace Alpha.BetLists.Tests.Configurations
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using BetLists.Configurations;
    using Xunit;

    public class AppSettingsTests
    {
        [Fact]
        public void Verify_MissingStoreProcedurePath_ThrowsConfigurationErrorsException()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("ReportsAppPath", "site-reports");

            // Act & Assert
            Assert.Throws<ConfigurationErrorsException>(() => AppSettings.Verify());
        }

        [Fact]
        public void Verify_HasEnoughSettings_PassVerify()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("StoredProcedurePath", "Local");
            AppSettings.AppSettingValues.Add("SiteLabel", "xxx");
            var passVerify = false;

            // Act
            AppSettings.Verify();
            passVerify = true;

            // Assert
            Assert.True(passVerify);
        }

        [Fact]
        public void DefaultIpAddress_Always_ReturnsCorrectConfig()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("DefaultIpAddress", "::1");
            var expectedDefaultIpAddress = "::1";

            // Act
            var actualDefaultIpAddress = AppSettings.DefaultIpAddress;

            // Assert
            Assert.Equal(expectedDefaultIpAddress, actualDefaultIpAddress);
        }

        [Fact]
        public void LocalIpAddress_Always_ReturnsCorrectConfig()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("LocalIpAddress", "127.0.0.1");
            var expectedLocalIpAddress = "127.0.0.1";

            // Act
            var actualLocalIpAddress = AppSettings.LocalIpAddress;

            // Assert
            Assert.Equal(expectedLocalIpAddress, actualLocalIpAddress);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void EnableBetsListMonitoring_Always_ReturnsCorrectConfig(string enableBetsListMonitoringSetting, bool expectedEnableBetsListMonitoring)
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("EnableBetsListMonitoring", enableBetsListMonitoringSetting);

            // Act
            var actualEnableBetsListMonitoring = AppSettings.EnableBetsListMonitoring;

            // Assert
            Assert.Equal(expectedEnableBetsListMonitoring, actualEnableBetsListMonitoring);
        }

        [Fact]
        public void UsersAlwaysAllowAccessBetsListMonitoring_Always_ReturnsCorrectConfig()
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("UsersAlwaysAllowAccessBetsListMonitoring", "1176086");
            var expectedUsersAlwaysAllowAccessBetsListMonitoring = new List<int> { 1176086 };

            // Act
            var actualUsersAlwaysAllowAccessBetsListMonitoring = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;

            // Assert
            Assert.Equal(expectedUsersAlwaysAllowAccessBetsListMonitoring, actualUsersAlwaysAllowAccessBetsListMonitoring);
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("0", false)]
        [InlineData("", false)]
        public void LocalTest_Always_ReturnsCorrectConfig(string value, bool expectedSetting)
        {
            // Arrange
            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("LocalTest", value);

            // Act
            var localTest = AppSettings.LocalTest;

            // Assert
            Assert.Equal(expectedSetting, localTest);
        }
    }
}