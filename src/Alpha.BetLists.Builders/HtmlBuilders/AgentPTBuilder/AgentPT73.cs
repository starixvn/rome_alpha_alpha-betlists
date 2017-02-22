namespace Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Templates;

    /// <summary>
    /// Build AgentPT
    /// </summary>
    public class AgentPT73 : AgentPT1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData"></param>
        /// <returns></returns>
        public override AgentPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            // Do not show percent of PT & Commission for this type of ticket

            this.Template.AgentPT.Visible = false;

            this.Template.AgentComm.Visible = false;

            return Template;
        }
    }
}