namespace Alpha.BetLists.Builders.Tests.HtmlBuilders
{
    using System.Linq;
    using BetLists.Tests.TestData;
    using Builders.HtmlBuilders;
    using Builders.PTComm;
    using Builders.Templates;
    using Fanex.BetList.Core.Entities;
    using Models;
    using Templates;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class BetListHtmlBuilderExtTests
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
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

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
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

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
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.False(actualTemplate.PT.Visible);
        }

        [Fact]
        public void BuildPTComm_PTValue_PTValueIsNotNullAndPTValueIsVisible_ShowPTValue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.Equal(positionTakingComm.PositionTaking, actualTemplate.PTValue.PT_Value);
        }

        [Fact]
        public void BuildPTComm_PTValue_PTValueIsNullOrPTValueIsNotVisible_HidePTValue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = BetStatus.Running;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.False(actualTemplate.PTValue.Visible);
        }

        [Fact]
        public void BuildPTComm_CommPercent_CommPercentIsNotNullAndCommPercentIsVisible_ShowCommPercent()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.Equal(positionTakingComm.CommissionPercent, actualTemplate.Comm.Comm_Percent);
        }

        [Fact]
        public void BuildPTComm_CommPercent_CommPercentIsNullOrCommPercentIsNotVisible_HideCommPercent()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.BetTypeId = 71;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.False(actualTemplate.Comm.Visible);
        }

        [Fact]
        public void BuildPTComm_CommValue_CommValueIsNotNullAndCommValueIsVisible_ShowCommValue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.Equal(positionTakingComm.Commission, actualTemplate.CommValue.Comm_value);
        }

        [Fact]
        public void BuildPTComm_CommValue_CommValueIsNullOrCommValueIsNotVisible_HideCommValue()
        {
            // Arrange
            var ticket = BetListDataTest.Ticket;
            ticket.Status = BetStatus.Running;
            var positionTakingComm
                = PTCommFactory.CreateInstance(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Act
            var actualTemplate = BetListHtmlBuilderExt.BuildPtComm(ticket, BetListDataTest.TicketData.ToList<ITicketData>(), UserLevel.Agent);

            // Assert
            Assert.False(actualTemplate.CommValue.Visible);
        }
    }
}