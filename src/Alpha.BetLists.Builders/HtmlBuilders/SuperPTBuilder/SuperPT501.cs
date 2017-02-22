namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    /// <summary>
    /// Build SuperPT
    /// </summary>
    public class SuperPT501 : SuperPT1
    {
        public override SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
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

            Template.SuperComm.superComm_setting = (ticket.CommStatus == "1" ?
                string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.SuperDiscount) }) :
                string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.SuperDiscount) })) + "%";

            bool isIMBCMatch = ticket.SuperDiscount < 0 || ticket.MasterDiscount < 0 || ticket.AgentDiscount < 0;
            if (isIMBCMatch)
            {
                Template.SuperCommValue.Visible = false;
                Template.SuperComm.Visible = false;
            }

            return Template;
        }
    }
}