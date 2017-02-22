namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    /// <summary>
    /// Build SuperPT
    /// </summary>
    public class SuperPT1 : ISuperPT
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public SuperPT_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public virtual SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            Template.SuperPT.superPT_setting = string.Join(null, new string[] { ConvertByBetType.Pt(ticket.SuperPositionTaking), "%" });

            string status = ticket.Status.ToLower();

            if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
            {
                Template.SuperPTValue.Visible = false;
                Template.SuperCommValue.Visible = false;
            }
            else
            {
                Template.SuperPTValue.superPT_value = ConvertByBetType.PtValue(ticket.SuperWinlost);
                Template.SuperCommValue.superComm_value = ConvertByBetType.Comm(ticket.SuperComm);
            }

            Template.SuperComm.superComm_setting = (ticket.CommStatus == "1" ? string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.SuperDiscount) }) : string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.SuperDiscount) })) + "%";

            return Template;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperPT1" /> class.
        /// </summary>
        public SuperPT1()
        {
            Template = new SuperPT_Template();
        }
    }
}