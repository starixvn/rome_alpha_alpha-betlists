namespace Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MasterPT1 : IMasterPT
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public MasterPT_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public virtual MasterPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            Template.MasterPT.masterPT_setting = string.Join(null, new string[] { ConvertByBetType.Pt(ticket.MasterPositionTaking), "%" });

            string status = ticket.Status.ToLower();

            if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
            {
                Template.MasterPTValue.Visible = false;
                Template.MasterCommValue.Visible = false;
            }
            else
            {
                Template.MasterPTValue.masterPT_value = ConvertByBetType.PtValue(ticket.MasterWinlost);
                Template.MasterCommValue.masterComm_value = ConvertByBetType.Comm(ticket.AgentComm);
            }

            Template.MasterComm.masterComm_setting = (ticket.CommStatus == "1" ? string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.MasterDiscount) }) : string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.MasterDiscount) })) + "%";

            return Template;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterPT1" /> class.
        /// </summary>
        public MasterPT1()
        {
            Template = new MasterPT_Template();
        }
    }
}