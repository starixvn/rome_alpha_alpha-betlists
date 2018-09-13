using System.Collections.Generic;

namespace Alpha.BetLists.Constants
{
    public static class TicketStatusIdConstants
    {
        public const int Reject = 101;
        public const int Void = 102;
        public const int Refund = 103;

        public static readonly List<int> All = new List<int>{ Reject, Void, Refund };
    }
}
