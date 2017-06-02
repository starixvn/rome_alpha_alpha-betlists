using Alpha.BetLists.Services;
using Xunit;

namespace Alpha.BetLists.Tests.Services
{
    public class EncryptionServiceImplTests
    {
        [Fact]
        public void CfsEncode_Always_ReturnsCorrectString()
        {
            // Arrange
            var service = new EncryptionServiceImpl();
            var codeStr = "codeStr";
            var number = 6;
            var expectedString = "C5E8266480B8B888B";

            // Act
            var actualString = service.CfsEncode(codeStr, number);

            // Assert
            Assert.Equal(expectedString, actualString);
        }
    }
}
