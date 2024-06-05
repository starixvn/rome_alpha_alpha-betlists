using System.Collections.Generic;

namespace Alpha.BetLists.Constants
{
    public static class ProductDetailIdConstants
    {
        public const int Sportsbook = 11;
        public const int Soccer = 12;
        public const int Basketball = 13;
        public const int Cricket = 14;
        public const int Tennis = 20;
        public const int IceHockey = 23;

        public const int SoccerPinGoal = 15;
        public const int BasketballPinGoal = 16;
        public const int EsportsPinGoal = 19;
        public const int SabaOtherSports = 24;

        public static readonly IEnumerable<int> ExcludedProductDetailFor777 = new List<int>
        {
            SoccerPinGoal,
            BasketballPinGoal,
            EsportsPinGoal
        };
    }
}