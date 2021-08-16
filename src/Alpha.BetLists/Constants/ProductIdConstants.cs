using System.Collections.Generic;

namespace Alpha.BetLists.Constants
{
    public static class ProductIdConstants
    {
        public const int Sportsbook = 1;
        public const int NumberGame = 3;
        public const int VirtualSports = 5;
        public const int RNGCasino = 6;
        public const int Keno = 8;
        public const int GoldDeluxe = 12;
        public const int NG2 = 23;
        public const int FishingWorld = 24;
        public const int Lottery = 28;
        public const int VirtualSports2 = 27;
        public const int VirtualGames = 38;
        public const int TableGames = 40;
        public const int SexyGaming = 45;
        public const int Pragmatic = 32;
        public const int SportsLottery = 63;
        public const int Bitcoin = 69;

        public static readonly List<int> ProductListHasProductDetail = new List<int>
        {
            ProductIdConstants.Pragmatic,
            ProductIdConstants.SexyGaming
        };
    }
}