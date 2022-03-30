namespace Alpha.BetLists.Tests.Models
{
    using System;
    using BetLists.Models;
    using Xunit;

    public class CashoutHistoryDetailTests
    {
        [Fact]
        public void ActionName_InCorrectId_ReturnsId()
        {
            // Arrange
            var incorrectId = "0";
            var model = new CashoutHistoryDetail(DateTime.Now, 10, incorrectId);

            // Act
            var actual = model.ActionName;

            // Assert
            Assert.Equal(incorrectId, actual);
        }

        [Theory]
        [InlineData("1", "Cash Out")]
        [InlineData("6", "Cash Out Cancelled")]
        [InlineData("8", "Cash Out Recovered")]
        public void ActionName_CorrectId_ReturnsName(string id, string name)
        {
            // Arrange
            var model = new CashoutHistoryDetail(DateTime.Now, 10, id);

            // Act
            var actual = model.ActionName;

            // Assert
            Assert.Equal(name, actual);

        }
    }
}