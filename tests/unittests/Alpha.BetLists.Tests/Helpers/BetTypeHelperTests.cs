using Alpha.BetLists.Helpers;
using Alpha.BetLists.Tests.Constants;
using Xunit;

namespace Alpha.BetLists.Tests.Helpers
{
    public class BetTypeHelperTests
    {
        [Theory]
        [InlineData(BetTypeIdConstants.HorseRacing_WinPlace, true)]
        [InlineData(BetTypeIdConstants.VirtualHorseRacing_WinPlace, true)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue, false)]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(-1, false)]
        public void IsWinPlace_Always_ReturnsCorrectValue(int betType, bool expectedValue)
        {
            // Act
            var actualValue = BetTypeHelper.IsWinPlace(betType);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
