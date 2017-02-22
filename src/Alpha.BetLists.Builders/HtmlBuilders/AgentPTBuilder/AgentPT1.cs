namespace Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    /// <summary>
    /// Build AgentPT
    /// </summary>
    public class AgentPT1 : IAgentPT
    {
        public AgentPT_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public virtual AgentPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            Template.AgentPT.agentPT_setting = string.Join(null, new string[] { ConvertByBetType.Pt(ticket.AgentPositionTaking), "%" });

            string status = ticket.Status.ToLower();

            if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
            {
                Template.AgentPTValue.Visible = false;
                Template.AgentCommValue.Visible = false;
            }
            else
            {
                Template.AgentPTValue.agentPT_value = ConvertByBetType.PtValue(ticket.AgentWinlost);
                Template.AgentCommValue.agentComm_value = ConvertByBetType.Comm(ticket.Comm);
            }

            Template.AgentComm.agentComm_setting = (ticket.CommStatus == "1" ? string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.AgentDiscount) }) : string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.AgentDiscount) })) + "%";

            return Template;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentPT1" /> class.
        /// </summary>
        public AgentPT1()
        {
            this.Template = new AgentPT_Template();
        }
    }
}