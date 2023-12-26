namespace Alpha.BetLists.Site.Tests.Models
{
    using System.Collections.Specialized;
    using Alpha.BetLists.Configurations;
    using AlphaPlus.User.Models;
    using NSubstitute;
    using Site.Models;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    public class UserTests
    {
        private readonly IUserSession userSession;

        public UserTests()
        {
            userSession = Substitute.For<IUserSession>();
        }

        [Fact]
        public void Id_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.CustId.Returns(1);

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal(1, user.Id);
        }

        [Fact]
        public void Name_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.UserName.Returns("harrison");

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal("harrison", user.Name);
        }

        [Fact]
        public void Language_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.Language.Returns("en-US");

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal("en-US", user.Language);
        }

        [Fact]
        public void LevelId_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.RoleId.Returns((byte)2);

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal(2, user.LevelId);
        }

        [Fact]
        public void AdminId_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.AdminId.Returns(2);

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal(2, user.AdminId);
        }

        [Fact]
        public void IsInternal_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.IsInternal.Returns(false);

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal(false, user.IsInternal);
        }

        [Fact]
        public void IsAdminCanViewVNIP_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.IsAdminCanViewVNIP.Returns(false);

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal(false, user.IsAdminCanViewVNIP);
        }

        [Fact]
        public void LangKey_Always_SetValueFromUserSession()
        {
            // Arrange
            userSession.LangKey.Returns("en");

            // Act
            var user = new User(userSession);

            // Assert
            Assert.Equal("en", user.LangKey);
        }

        [Fact]
        public void IsEnabledJackpot_JackpotEnabled_ReturnTrue()
        {
            // Arrange
            var userSession = Substitute.For<IUserSession>();
            userSession.GetSyncedProduct(6).Returns(true);
            var user = new UserForTest(userSession);

            // Act
            var value = user.IsEnabledJackpot;

            // Assert
            Assert.True(value);
        }
    }
}
