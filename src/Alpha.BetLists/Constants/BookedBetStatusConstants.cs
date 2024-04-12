using Alpha.BetLists.AppResources;
using System.Collections.Generic;

namespace Alpha.BetLists.Constants
{
    public static class BookedBetStatusConstants
    {
        public const int Booking = 0;
        public const int Successful = 1;
        public const int Unsuccessful = 2;
        public const int ProcressingBet = 3;
        public const int Cancelled = 4;

        public static readonly Dictionary<int, string> Dropdownlist = new Dictionary<int, string>
        {
            { Booking, Resources.Booking },
            { Successful, Resources.Successful },
            { Unsuccessful, Resources.Unsuccessful },
            { Cancelled, Resources.Cancelled }
        };

        public static readonly Dictionary<int, string> All = new Dictionary<int, string>
        {
            { Booking, Resources.Booking },
            { Successful, Resources.Successful },
            { Unsuccessful, Resources.Unsuccessful },
            { Cancelled, Resources.Cancelled },
            { ProcressingBet, Resources.Booking }
        };
    }
}
