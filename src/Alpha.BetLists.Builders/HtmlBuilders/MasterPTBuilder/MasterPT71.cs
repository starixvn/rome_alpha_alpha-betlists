namespace Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Templates;

    public class MasterPT71 : MasterPT1
    {
        public override MasterPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            //// Do not show percent of PT & Commission for this type of ticket

            this.Template.MasterPT.Visible = false;

            this.Template.MasterComm.Visible = false;

            return this.Template;
        }
    }
}