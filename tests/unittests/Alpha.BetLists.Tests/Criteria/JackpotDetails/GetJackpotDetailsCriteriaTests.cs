namespace Alpha.BetLists.Tests.Criteria.JackpotDetails
{
    using BetLists.Criteria.JackpotDetails;
    using Xunit;

    [Trait("Category", "JackpotDetails")]
    public class GetJackpotDetailsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectKey()
        {
            // Arrange
            var criteria = new GetJackpotDetailsCriteria(0, 0, 0);
            var expectedSettingKey = "Acc_BetList_Jackpot_Detail";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidTransId_ReturnsFalse(long transId)
        {
            // Arrange
            var criteria = new GetJackpotDetailsCriteria(transId, 1, 1);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetJackpotDetailsCriteria(1, custId, 1);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void IsValid_InvalidBetType_ReturnsFalse(int betType)
        {
            // Arrange
            var criteria = new GetJackpotDetailsCriteria(1, 1, betType);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetJackpotDetailsCriteria(1, 1, 1);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
}