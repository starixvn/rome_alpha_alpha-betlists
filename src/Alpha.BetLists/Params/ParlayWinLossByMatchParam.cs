namespace Alpha.BetLists.Params
{
    using System;
    using Models;

    public class ParlayWinLossByMatchParam
    {
        public ParlayWinLossByMatchParam(
            int custId,
            int betType,
            string language,
            DateTime fromDate,
            DateTime toDate,
            DateTime limtedDate,
            UserLevel custLevel)
        {
            CustId = custId;
            BetType = betType;
            Language = language;
            FromDate = fromDate;
            ToDate = toDate;
            CustLevel = custLevel;
            LimitedDate = limtedDate;
        }

        public int CustId { get; }

        public int BetType { get; }

        public string Language { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public UserLevel CustLevel { get; }

        public DateTime LimitedDate { get; }
    }
}
