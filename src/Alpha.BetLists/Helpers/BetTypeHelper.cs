namespace Alpha.BetLists.Helpers
{
    using System.Linq;
    using Constants;

    public static class BetTypeHelper
    {
        public static readonly int[] HorseRacingBetType =
        {
            BetTypeIdConstants.VirtualHorseRacing_Win,
            BetTypeIdConstants.VirtualHorseRacing_Place,
            BetTypeIdConstants.VirtualHorseRacing_WinPlace
        };

        public static bool IsOutRight(int betType) => betType == BetTypeIdConstants.Outright;

        public static bool IsHorseRacing(int betType) => HorseRacingBetType.Contains(betType);
    }
}