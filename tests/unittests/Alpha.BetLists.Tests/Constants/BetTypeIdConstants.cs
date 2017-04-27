using System.Diagnostics.CodeAnalysis;

namespace Alpha.BetLists.Tests.Constants
{
    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1310:FieldNamesMustNotContainUnderscore",
        Justification = "Reviewed. Suppression is OK here.")]
    public static class BetTypeIdConstants
    {
        public const int Outright = 10;

        public const int HorseRacing_Win = 31;
        public const int HorseRacing_Place = 32;
        public const int HorseRacing_WinPlace = 33;

        public const int HorseRacingTote_Win = 41;
        public const int HorseRacingTote_Place = 42;
        public const int HorseRacingTote_WinPlace = 43;

        public const int VirtualHorseRacing_Win = 1231;
        public const int VirtualHorseRacing_Place = 1232;
        public const int VirtualHorseRacing_WinPlace = 1233;
    }
}
