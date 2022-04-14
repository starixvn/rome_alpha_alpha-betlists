namespace Alpha.BetLists.Params
{
    using System;
    using System.Collections.Generic;

    public class CashOutHistoryParam
    {
        public CashOutHistoryParam(
            long transId,
            int custId,
            DateTime winlostDate)
        {
            TransId = transId;
            CustId = custId;
            WinlostDate = winlostDate;
        }

        public long TransId { get; }

        public int CustId { get; }

        public DateTime WinlostDate { get; }
        
    }
}