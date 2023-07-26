namespace Alpha.BetLists.Params
{
    using System;

    public class FindTicketsParam
    {
        public FindTicketsParam(
            long refNo)        {
            RefNo = refNo;
        }

        public long RefNo { get; }

    }
}
