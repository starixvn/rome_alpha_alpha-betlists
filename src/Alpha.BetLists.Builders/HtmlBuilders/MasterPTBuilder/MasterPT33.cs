namespace Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using AppResources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    public class MasterPT33 : MasterPT1
    {
        public MasterPT33()
        {
            WinBetId = 31;
            PlaceBetId = 32;
        }

        protected int WinBetId { get; set; }

        protected int PlaceBetId { get; set; }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        public override MasterPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal masterPT_Win = 0;
            decimal masterPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                masterPT_Win = refTicketData.FindLast(item => item.BetTypeId == WinBetId).MasterPT;
                masterPT_Place = refTicketData.FindLast(item => item.BetTypeId == PlaceBetId).MasterPT;
            }

            this.Template.MasterPT.masterPT_setting = string.Join(
                null,
                new string[]
                {
                    "<span class='blue' style='font-weight:normal;'>",
                    Resources.Win + ": ",
                    "<b>",
                    Formatter.FormatNumber2(masterPT_Win * 100, null),
                    "%</b>",
                    "</span><br>",
                    "<span class='' style='font-weight:normal;'>",
                    Resources.place + ": ",
                    "<b>",
                    Formatter.FormatNumber2(masterPT_Place * 100, null),
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