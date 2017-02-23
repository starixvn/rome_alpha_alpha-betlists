namespace Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    public class MasterPT500 : MasterPT1
    {
        public override MasterPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

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

            var discount = -(ticket.MasterDiscount - ticket.AgentDiscount);
            Template.MasterComm.masterComm_setting = (ticket.CommStatus == "1" ?
                string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(discount) }) :
                string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(discount) })) + "%";

            return Template;
        }
    }
}