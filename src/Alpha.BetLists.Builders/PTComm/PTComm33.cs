namespace Alpha.BetLists.Builders.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class PTComm33 : PTComm
    {
        public PTComm33(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
            : base(ticket, userLevel)
        {
            TicketData = ticketData;
            PositionTakingWin = GetPositionTakingByUserLevel(userLevel, isWin: true);
            PositionTakingPlace = GetPositionTakingByUserLevel(userLevel, isWin: false);
        }

        protected List<ITicketData> TicketData { get; }

        public int WinBetId => !IsVirtualRacing ? 31 : 1231;

        public int PlaceBetId => !IsVirtualRacing ? 32 : 1232;

        private bool IsVirtualRacing => Ticket.BetTypeId == 1233;

        private string GetPositionTakingByUserLevel(UserLevel userLevel, bool isWin)
        {
            var refTicketData = IsVirtualRacing ?
                TicketData.FindAll(item => item.RefNo.Equals(Ticket.TransId.ToString())) :
                TicketData.FindAll(item => item.RefNo.Equals(Ticket.RefNo));

            decimal positionTaking;

            if (userLevel.IsSuperLevel())
            {
                positionTaking = refTicketData.LastOrDefault(item => item.BetTypeId == (isWin ? WinBetId : PlaceBetId)).SuperPT;
            }
            else if (userLevel.IsMasterLevel())
            {
                positionTaking = refTicketData.LastOrDefault(item => item.BetTypeId == (isWin ? WinBetId : PlaceBetId)).MasterPT;
            }
            else
            {
                positionTaking = refTicketData.LastOrDefault(item => item.BetTypeId == (isWin ? WinBetId : PlaceBetId)).AgentPT;
            }

            return Formatter.FormatNumber2(positionTaking, null);
        }
    }
}