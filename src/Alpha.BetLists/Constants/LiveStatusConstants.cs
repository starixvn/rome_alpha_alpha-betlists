namespace Alpha.BetLists.Constants
{
    public static class LiveStatusConstants
    {
        public const byte All = 3;

        public const byte Mix = 2;

        public const byte Live = 1;

        public const byte NonLive = 0;

        public static string GetStatusById(byte id) => id == All ? "All" : id.ToString();

    }
}
