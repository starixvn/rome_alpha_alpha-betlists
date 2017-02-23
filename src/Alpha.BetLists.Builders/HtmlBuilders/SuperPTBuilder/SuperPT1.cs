namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    public class SuperPT1 : ISuperPT
    {
        public SuperPT1()
        {
            Template = new SuperPT_Template();
        }

        public SuperPT_Template Template { get; set; }

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

            Template.SuperComm.superComm_setting = (ticket.CommStatus == "1"
                ? string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.SuperDiscount) })
                : string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.SuperDiscount) })) + "%";

            return Template;
        }
    }
}