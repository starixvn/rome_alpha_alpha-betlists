namespace Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using AppResources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    public class AgentPT33 : AgentPT1
    {
        public AgentPT33()
        {
            WinBetId = 31;
            PlaceBetId = 32;
        }

        protected int WinBetId { get; set; }

        protected int PlaceBetId { get; set; }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        public override AgentPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal agentPT_Win = 0;
            decimal agentPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                agentPT_Win = refTicketData.FindLast(item => item.BetTypeId == WinBetId).AgentPT;
                agentPT_Place = refTicketData.FindLast(item => item.BetTypeId == PlaceBetId).AgentPT;
            }

            this.Template.AgentPT.agentPT_setting = string.Join(
                null,
                new string[]
                {
                    "<span class='blue' style='font-weight:normal;'>",
                    Resources.Win + ": ",
                    "<b>",
                    Formatter.FormatNumber2(agentPT_Win * 100, null),
                    "%</b>",
                    "</span><br>",
                    "<span class='' style='font-weight:normal;'>",
                    Resources.place + ": ",
                    "<b>",
                    Formatter.FormatNumber2(agentPT_Place * 100, null),
                    "%</b>",
                    "</span>"
                });

            return this.Template;
        }

        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }
    }
}