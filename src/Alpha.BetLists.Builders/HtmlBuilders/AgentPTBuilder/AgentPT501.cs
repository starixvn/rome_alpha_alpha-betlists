namespace Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    /// <summary>
    /// Build AgentPT
    /// </summary>
    public class AgentPT501 : AgentPT1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData"></param>
        /// <returns></returns>
        public override AgentPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
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

            Template.AgentComm.agentComm_setting = (ticket.CommStatus == "1" ?
                string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.AgentDiscount) }) :
                string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.AgentDiscount) })) + "%";

            bool isIMBCMatch = ticket.SuperDiscount < 0 || ticket.MasterDiscount < 0 || ticket.AgentDiscount < 0;
            if (isIMBCMatch)
            {
                Template.AgentCommValue.Visible = false;
                Template.AgentComm.Visible = false;
            }

            return Template;
        }
    }
}