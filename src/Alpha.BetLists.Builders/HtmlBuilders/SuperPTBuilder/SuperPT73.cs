namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Templates;

    public class SuperPT73 : SuperPT1
    {
        public override SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            //// Do not show percent of PT & Commission for this type of ticket

            this.Template.SuperPT.Visible = false;

            this.Template.SuperComm.Visible = false;

            return this.Template;
        }
    }
}