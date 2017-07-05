namespace Alpha.BetLists.Params
{
    using System;

    public class DailyStatementParam
    {
        public DailyStatementParam(
            int custId,
            DateTime winlossDate,
            int transType,
            string language)
        {
            CustId = custId;
            WinLossDate = winlossDate;
            TransType = transType;
            Language = language;
        }

        public int CustId { get; }

        public DateTime WinLossDate { get; }

        public int TransType { get; }

        public string Language { get; }
    }
}
