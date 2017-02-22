namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Templates;

    /// <summary>
    /// Build SuperPT
    /// </summary>
    public class SuperPT73 : SuperPT1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData"></param>
        /// <returns></returns>
        public override SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            // Do not show percent of PT & Commission for this type of ticket

            this.Template.SuperPT.Visible = false;

            this.Template.SuperComm.Visible = false;

            return this.Template;
        }
    }
}