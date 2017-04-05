namespace Alpha.BetLists.Builders.Models
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;

    public class PTComm501 : PTComm
    {
        public PTComm501(ITicket ticket, UserLevel userLevel) : base(ticket, userLevel)
        {
        }

        public override bool VisibleCommision => IsIMBCMatch ? false : base.VisibleCommision;

        public override bool VisibileCommisionPercent => IsIMBCMatch ? false : base.VisibileCommisionPercent;

        private bool IsIMBCMatch => Ticket.SuperDiscount < 0 || Ticket.MasterDiscount < 0 || Ticket.AgentDiscount < 0;
    }
}