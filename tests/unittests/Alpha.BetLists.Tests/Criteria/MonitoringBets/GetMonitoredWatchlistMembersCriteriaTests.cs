namespace Alpha.BetLists.Tests.Criteria.MonitoringBets
{
    using Alpha.BetLists.Criteria.MonitoringBets;
    using Alpha.BetLists.Models.MonitoringBets;
    using Alpha.BetLists.Params.MonitoringBets;
    using Xunit;

    [Trait("Category", "GetMonitoredWatchlistMembers")]
    public class GetMonitoredWatchlistMembersCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetMonitoredWatchlistMembersCriteria(1);

            var expectedSettingKey = "Acc_WLMonitored_Player_WatchList_Sel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_ValidSportParam_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetMonitoredWatchlistMembersCriteria(1);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }


        [Fact]
        public void IsValid_InvalidParam_ReturnsFalse()
        {
            // Arrange
            var criteria = new GetMonitoredWatchlistMembersCriteria(0);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }
    }
}