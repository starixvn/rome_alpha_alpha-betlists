namespace Alpha.BetLists.Helpers
{
    using System.Linq;
    using Constants;

    public static class BetTypeHelper
    {
        public static readonly int[] HorseRacingBetType =
        {
            BetTypeIdConstants.HorseRacing_Win,
            BetTypeIdConstants.HorseRacing_Place,
            BetTypeIdConstants.HorseRacing_WinPlace,

            BetTypeIdConstants.HorseRacingTote_Win,
            BetTypeIdConstants.HorseRacingTote_Place,
            BetTypeIdConstants.HorseRacingTote_WinPlace,

            BetTypeIdConstants.VirtualHorseRacing_Win,
            BetTypeIdConstants.VirtualHorseRacing_Place,
            BetTypeIdConstants.VirtualHorseRacing_WinPlace
        };

        public static bool IsOutRight(int betType) => betType == BetTypeIdConstants.Outright;

        public static bool IsHorseRacing(int betType) => HorseRacingBetType.Contains(betType);
    }
}