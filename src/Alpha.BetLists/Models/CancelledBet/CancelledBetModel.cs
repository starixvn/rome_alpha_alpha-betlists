using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.BetLists.Models.CancelledBet
{
    public class CancelledBetModel
    {
        public CancelledBetModel(FilterCancelledBetModel filter, BetListData betListData)
        {
            Filter = filter;
            BetListData = betListData;
        }

        public FilterCancelledBetModel Filter { get; set; }

        public BetListData BetListData { get; set; }
    }
}
