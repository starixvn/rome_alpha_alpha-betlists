namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    public class SuperPT500 : SuperPT1
    {
        public override SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);
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

            var discount = -(ticket.SuperDiscount - ticket.MasterDiscount);
            Template.SuperComm.superComm_setting = (ticket.CommStatus == "1" ?
                string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(discount) }) :
                string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(discount) })) + "%";

            return Template;
        }
    }
}