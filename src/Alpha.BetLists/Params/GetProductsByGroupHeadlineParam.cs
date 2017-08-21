namespace Alpha.BetLists.Params
{
    using System;

    public class GetProductsByGroupHeadlineParam
    {
        public GetProductsByGroupHeadlineParam(int custId, DateTime minDate)
        {
            CustId = custId;
            MinDate = minDate;
        }

        public int CustId { get; }

        public DateTime MinDate { get; }
    }
}
