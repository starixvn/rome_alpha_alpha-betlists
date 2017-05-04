namespace Alpha.BetLists.Builders.Tests.HtmlBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BetLists.Tests.TestData;
    using Builders.HtmlBuilders;
    using Builders.PTComm;
    using Fanex.BetList.Core.Entities;
    using Models;
    using Templates;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class BetListHtmlBuildersTests
    {
        [Theory]
        [InlineData(33)]
        [InlineData(1233)]
        public void BuildPTComm_PTPercent_BetTypeIsRacing_RenderRacingPTValue(int betTypeId)
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = betTypeId;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent) as PTComm33;
            var expectedPTPercent = new PTRacing_Template();
            expectedPTPercent.PT_Win = positionTakingComm.PositionTakingWin;
            expectedPTPercent.PT_Place = positionTakingComm.PositionTakingPlace;

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPTComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.Equal(expectedPTPercent, actualTemplate.PT.PT_Percent);
        }

        [Fact]
        public void BuildPTComm_PTPercent_PTPercentIsNotNullAndPTPercentIsVisible_ShowPTPercent()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPTComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.Equal(positionTakingComm.PositionTakingPercent, actualTemplate.PT.PT_Percent);
        }

        [Fact]
        public void BuildPTComm_PTPercent_PTPercentIsNullOrPTPercentIsNotVisible_HidePTPercent()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = 71; // Bet type 71 will hide PT Percent

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPTComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.False(actualTemplate.PT.Visible);
        }
    }
}