namespace Alpha.BetLists.Site.Tests.Request
{
    using System;
    using BetLists.Models;
    using Fixtures;
    using NSubstitute;
    using Requests.WinLoss;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class BaseQueryTests
    {
        private readonly IUser user;

        public BaseQueryTests(BetListFixture betListFixture)
        {
            user = betListFixture.User;
        }

        [Fact]
        public void CustId_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                CustId = 1
            };

            // Assert
            Assert.Equal(1, query.CustId);
        }

        [Fact]
        public void CustName_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                CustName = "harrison"
            };

            // Assert
            Assert.Equal("harrison", query.CustName);
        }

        [Fact]
        public void FromDate_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                FromDate = DateTime.Today
            };

            // Assert
            Assert.Equal(DateTime.Today, query.FromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                ToDate = DateTime.Today
            };

            // Assert
            Assert.Equal(DateTime.Today, query.ToDate);
        }

        [Fact]
        public void ProductIds_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                ProductIds = "1"
            };

            // Assert
            Assert.Equal("1", query.ProductIds);
        }

        [Fact]
        public void Language_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                Language = "en"
            };

            // Assert
            Assert.Equal("en", query.Language);
        }

        [Fact]
        public void ViewerId_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user);

            // Assert
            Assert.Equal(user.Id, query.ViewerId);
        }

        [Fact]
        public void ViewerLevelId_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user);

            // Assert
            Assert.Equal(user.LevelId, query.ViewerLevelId);
        }

        [Fact]
        public void ViewerLevel_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user);

            // Assert
            Assert.Equal(Enumeration.FromValue<UserLevel>(user.LevelId), query.ViewerLevel);
        }

        [Fact]
        public void ViewerIdOrViewerAdminId_IsInternal_ReturnAdminId()
        {
            // Arrange
            user.IsInternal.Returns(true);

            // Act
            var query = new WinLossQuery(user);

            // Assert
            Assert.Equal(user.AdminId, query.ViewerIdOrViewerAdminId);
        }

        [Fact]
        public void ViewerIdOrViewerAdminId_IsNotInternal_ReturnId()
        {
            // Arrange
            user.IsInternal.Returns(false);

            // Act
            var query = new WinLossQuery(user);

            // Assert
            Assert.Equal(user.Id, query.ViewerIdOrViewerAdminId);
        }

        [Fact]
        public void DisplayVNIP_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user);

            // Assert
            Assert.Equal(user.IsAdminCanViewVNIP, query.DisplayVNIP);
        }

        [Fact]
        public void Type_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                Type = "type"
            };

            // Assert
            Assert.Equal("type", query.Type);
        }

        [Fact]
        public void IsHistoricReport_Always_ReturnValue()
        {
            // Act
            var query = new WinLossQuery(user)
            {
                IsHistoricReport = false
            };

            // Assert
            Assert.Equal(false, query.IsHistoricReport);
        }
    }
}