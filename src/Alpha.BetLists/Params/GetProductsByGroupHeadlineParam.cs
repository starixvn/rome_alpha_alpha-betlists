using System;

namespace Alpha.BetLists.Params
{
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
