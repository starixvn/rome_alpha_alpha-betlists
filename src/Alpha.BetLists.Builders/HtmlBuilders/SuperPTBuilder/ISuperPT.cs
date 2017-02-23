namespace Alpha.BetLists.Builders.HtmlBuilders.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Templates;

    public interface ISuperPT
    {
        SuperPT_Template Template { get; set; }

        SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData);
    }
}