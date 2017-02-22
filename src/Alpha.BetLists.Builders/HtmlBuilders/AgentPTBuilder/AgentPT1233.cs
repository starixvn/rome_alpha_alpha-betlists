namespace Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentPT1233 : AgentPT33
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentPT1233" /> class.
        /// </summary>
        public AgentPT1233()
        {
            winBetId = 1231;
            placeBetId = 1232;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns></returns>
        protected override List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.RefNo));
        }
    }
}