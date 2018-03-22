namespace Alpha.BetLists.Tests.Constants
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage(
        "StyleCop.CSharp.NamingRules",
        "SA1310:FieldNamesMustNotContainUnderscore",
        Justification = "Reviewed. Suppression is OK here.")]
    public static class BetTypeIdConstants
    {
        public const int Handicap = 1;
        public const int Outright = 10;

        public const int VirtualHorseRacing_Win = 1231;
        public const int VirtualHorseRacing_Place = 1232;
        public const int VirtualHorseRacing_WinPlace = 1233;
    }
}
