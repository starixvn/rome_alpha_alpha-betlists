namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using AppResources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    public class SuperPT33 : SuperPT1
    {
        public SuperPT33()
        {
            WinBetId = 31;
            PlaceBetId = 32;
        }

        protected int WinBetId { get; set; }

        protected int PlaceBetId { get; set; }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        public override SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal superPT_Win = 0;
            decimal superPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                superPT_Win = refTicketData.FindLast(item => item.BetTypeId == WinBetId).SuperPT;
                superPT_Place = refTicketData.FindLast(item => item.BetTypeId == PlaceBetId).SuperPT;
            }

            this.Template.SuperPT.superPT_setting = string.Join(
                null,
                new string[]
                {
                    "<span class='blue' style='font-weight:normal;'>",
                    Resources.Win + ": ",
                    "<b>",
                    Formatter.FormatNumber2(superPT_Win * 100, null),
                    "%</b>",
                    "</span><br>",
                    "<span class='' style='font-weight:normal;'>",
                    Resources.place + ": ",
                    "<b>", Formatter.FormatNumber2(superPT_Place * 100, null),
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