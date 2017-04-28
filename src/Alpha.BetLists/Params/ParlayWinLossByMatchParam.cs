using System;
using Alpha.BetLists.Models;

namespace Alpha.BetLists.Params
{
    public class ParlayWinLossByMatchParam
    {
        public ParlayWinLossByMatchParam(
            int custId,
            int betType,
            string language,
            DateTime fromDate,
            DateTime toDate,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            Language = language;
            FromDate = fromDate;
            ToDate = toDate;
            CustLevel = custLevel;
        }

        public int CustId { get; }

        public int BetType { get; }

        public string Language { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public UserLevel CustLevel { get; }
    }
}
