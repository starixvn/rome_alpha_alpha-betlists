namespace Alpha.BetLists.Models.CancelledBet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FilterCancelledBetModel
    {
        public FilterCancelledBetModel()
        {
            Masters = new List<UserBetCount>();
            Agents = new List<UserBetCount>();
            Members = new List<UserBetCount>();
        }

        public IEnumerable<UserBetCount> Masters { get; set; }

        public IEnumerable<UserBetCount> Agents { get; set; }

        public IEnumerable<UserBetCount> Members { get; set; }

        public int MasterId { get; set; }

        public int AgentId { get; set; }

        public int MemberId { get; set; }
    }
}
