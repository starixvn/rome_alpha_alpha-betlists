namespace Alpha.BetLists.Builders.PTComm
{
    using System.Collections.Generic;
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;

    public class PTComm500 : PTComm
    {
        public PTComm500(ITicket ticket, UserLevel userLevel) : base(ticket, userLevel)
        {
        }

        protected override void InitCommSettingByUserLevel()
        {
            CommSettingByUserLevel = new Dictionary<UserLevel, object>
            {
                { UserLevel.Super, Ticket.SuperDiscount - Ticket.MasterDiscount },
                { UserLevel.Master, Ticket.MasterDiscount - Ticket.AgentDiscount },
                { UserLevel.Agent, -Ticket.AgentDiscount }
            };
        }
    }
}