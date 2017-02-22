namespace Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder
{
    using Fanex.BetList.Core.Entities;
    using Templates;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAgentPT
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        AgentPT_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns></returns>
        AgentPT_Template Render(ITicket ticket, System.Collections.Generic.List<ITicketData> ticketData);
    }
}