namespace Alpha.BetLists.Builders.PTComm
{
    using Fanex.BetList.Core.Entities;
    using Models;

    public class PTComm501 : PTComm
    {
        public PTComm501(ITicket ticket, UserLevel userLevel) : base(ticket, userLevel)
        {
        }

        public override bool VisibleCommision => !IsIMBCMatch && base.VisibleCommision;

        public override bool VisibileCommisionPercent => !IsIMBCMatch && base.VisibileCommisionPercent;

        private bool IsIMBCMatch => Ticket.SuperDiscount < 0 || Ticket.MasterDiscount < 0 || Ticket.AgentDiscount < 0;
    }
}