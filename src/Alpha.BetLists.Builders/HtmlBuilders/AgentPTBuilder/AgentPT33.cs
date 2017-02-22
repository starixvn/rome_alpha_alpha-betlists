namespace Alpha.BetLists.Builders.HtmlBuilders.AgentPTBuilder
{
    using System.Collections.Generic;
    using AppResources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Templates;

    public class AgentPT33 : AgentPT1
    {
        protected int winBetId, placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentPT33" /> class.
        /// </summary>
        public AgentPT33()
        {
            winBetId = 31;
            placeBetId = 32;
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData"></param>
        /// <returns></returns>
        public override AgentPT_Template Render(ITicket ticket, System.Collections.Generic.List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal agentPT_Win = 0;
            decimal agentPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                agentPT_Win = refTicketData.FindLast(item => item.BetTypeId == winBetId).AgentPT;
                agentPT_Place = refTicketData.FindLast(item => item.BetTypeId == placeBetId).AgentPT;
            }

            this.Template.AgentPT.agentPT_setting = string.Join(null, new string[]{ "<span class='blue' style='font-weight:normal;'>"
                                                                      ,Resources.Win + ": "
                                                                      ,"<b>",Formatter.FormatNumber2(agentPT_Win*100, null),"%</b>"
                                                                      ,"</span><br>"
                                                                      ,"<span class='' style='font-weight:normal;'>"
                                                                      ,Resources.place + ": "
                                                                      ,"<b>",Formatter.FormatNumber2(agentPT_Place*100, null),"%</b>"
                                                                      ,"</span>"});

            return this.Template;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns></returns>
        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }
    }
}