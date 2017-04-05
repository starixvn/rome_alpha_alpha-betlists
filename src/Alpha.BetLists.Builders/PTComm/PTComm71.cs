namespace Alpha.BetLists.Builders.Models
{
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;

    public class PTComm71 : PTComm
    {
        public PTComm71(ITicket ticket, UserLevel userLevel) : base(ticket, userLevel)
        {
        }

        public override bool VisibilePositionTakingPercent => false;

        public override bool VisibileCommisionPercent => false;
    }
}