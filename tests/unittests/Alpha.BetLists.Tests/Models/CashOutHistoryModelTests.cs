namespace Alpha.BetLists.Tests.Models
{
    using System;
    using BetLists.Models;
    using Xunit;

    public class CashOutHistoryModelTests
    {
        [Fact]
        public void DetailHistory_CashOutExtraInfoIsEmpty_ReturnEmpty()
        {
            // Arrange
            var model = new CashOutHistoryModel();
            model.CashoutExtraInfo = null;

            // Act
            var actual = model.DetailHistory;

            // Assert
            Assert.Equal(0, actual.Count);
        }

        [Fact]
        public void DetailHistory_CashOutExtraInfoIsValid_ReturnEmpty()
        {
            // Arrange
            var model = new CashOutHistoryModel
            {
                CashoutExtraInfo = "<root><r ct='2022-02-23 06:57:28.323' s='1' w='100'/></root>"
            };

            // Act
            var actual = model.DetailHistory;

            // Assert
            Assert.Equal(1, actual.Count);
        }
    }
}