namespace Alpha.BetLists.Builders.HtmlBuilders.MasterPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Templates;

    public interface IMasterPT
    {
        MasterPT_Template Template { get; set; }

        MasterPT_Template Render(ITicket ticket, List<ITicketData> ticketData);
    }
}