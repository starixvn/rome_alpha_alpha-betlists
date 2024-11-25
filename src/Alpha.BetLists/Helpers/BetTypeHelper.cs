namespace Alpha.BetLists.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.AppResources;
    using Constants;

    public static class BetTypeHelper
    {
        private static readonly int[] JackpotContributionBetType =
        {
            BetTypeIdConstants.JackpotContribution,
            BetTypeIdConstants.JackpotContribution_Mobile,
            BetTypeIdConstants.JackpotContribution_Tablet,
            BetTypeIdConstants.PPJackpotContribution,
            BetTypeIdConstants.SGJackpotContribution
        };

        private static readonly int[] HorseRacingBetType =
        {
            BetTypeIdConstants.VirtualHorseRacing_Win,
            BetTypeIdConstants.VirtualHorseRacing_Place,
            BetTypeIdConstants.VirtualHorseRacing_WinPlace
        };

        private static readonly IDictionary<byte, byte> LeagueTypeEventTypeMapping = new Dictionary<byte, byte>
        {
            { 1, 5 },
            { 0, 1 }
        };

        public static bool IsOutRight(int betType) => betType == BetTypeIdConstants.Outright;

        public static bool IsLeapGaming(int betType)
            => BetTypeIdConstants.LeapGamingMin == betType;

        public static bool IsHappy5LinkedGames(int betType)
            => BetTypeIdConstants.Happy5LinkedGamesMin <= betType && betType <= BetTypeIdConstants.Happy5LinkedGamesMax;

        public static bool IsHorseRacing(int betType) => HorseRacingBetType.Contains(betType);

        public static bool IsJackpotContribution(int betType) => JackpotContributionBetType.Contains(betType);

        public static string GetBetTypeName(int betTypeId)
            => betTypeId == BetTypeIdConstants.MonitoringBetsOthers
            ? Resources.Others
            : CacheRefServiceHelper.GetBetTypeName(betTypeId);

        public static byte GetEventType(byte leagueType)
        {
            const byte defaultEventType = 1;

            return LeagueTypeEventTypeMapping.ContainsKey(leagueType) ? LeagueTypeEventTypeMapping[leagueType] : defaultEventType;
        }
    }
}