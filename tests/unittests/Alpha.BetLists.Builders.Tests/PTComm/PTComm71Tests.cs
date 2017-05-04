namespace Alpha.BetLists.Builders.Tests.PTComm
{
    using BetLists.Tests.TestData;
    using Builders.PTComm;
    using Models;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class PTComm71Tests
    {
        [Fact]
        public void VisibilePositionTakingPercent_Always_ReturnFalse()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;

            // Act
            var actualPTComm = new PTComm71(ticket, UserLevel.Agent);

            // Assert
            Assert.False(actualPTComm.VisibilePositionTakingPercent);
        }

        [Fact]
        public void VisibileCommisionPercent_Always_ReturnFalse()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;

            // Act
            var actualPTComm = new PTComm71(ticket, UserLevel.Agent);

            // Assert
            Assert.False(actualPTComm.VisibileCommisionPercent);
        }
    }
}