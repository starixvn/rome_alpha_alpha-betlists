namespace Alpha.BetLists.Site.Tests.Request.LastBets
{
    using BetLists.Models;
    using NSubstitute;
    using Requests.LastBets;
    using Xunit;

    [Trait("Category", "LastBets")]
    public class LastBetsQueryTests
    {
        private readonly IUser user;

        public LastBetsQueryTests()
        {
            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);
        }

        [Fact]
        public void BetType_Default_ReturnsDefaultValue()
        {
            // Arrange
            var query = new LastBetsQuery(user);
            var expectedValue = -1;

            // Act
            var actualValue = query.BetType;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void PageSize_Always_ReturnsDefaultValue()
        {
            // Arrange
            var query = new LastBetsQuery(user);
            var expectedValue = 10;

            // Act
            var actualValue = query.PageSize;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
