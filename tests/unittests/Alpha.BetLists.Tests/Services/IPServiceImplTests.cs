namespace Alpha.BetLists.Tests.Services
{
    using Alpha.BetLists.Services;
    using Xunit;

    [Trait("Category", "Common")]
    public class IPServiceImplTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsVNIP_NullOrEmptyIP_ReturnsFalse(string ip)
        {
            //// Arrange
            var service = new IPServiceImpl();

            //// Act
            var actualValue = service.IsVNIP(ip);

            //// Assert
            Assert.False(actualValue);
        }
    }
}
