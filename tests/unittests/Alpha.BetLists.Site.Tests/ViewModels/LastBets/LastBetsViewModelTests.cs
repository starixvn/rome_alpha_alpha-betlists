using Alpha.BetLists.Models;
using Alpha.BetLists.Site.Requests.LastBets;
using Alpha.BetLists.Site.Tests.Fixtures;
using Alpha.BetLists.Site.ViewModels.LastBets;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.ViewModels.LastBets
{
    [Trait("Category", "LastBets")]
    [Collection("BetListCollection")]
    public class LastBetsViewModelTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly IUser user;

        public LastBetsViewModelTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            user = Substitute.For<IUser>();
            user.Id.Returns(2);
            user.LevelId.Returns(UserLevel.Agent.Value);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, false)]
        public void ResetLastTransId_Always_ReturnsCorrectValue(int betType, bool expectedResetLastTransId)
        {
            // Arrange
            var query = new LastBetsQuery(user)
            {
                BetType = betType
            };

            var viewModel = new LastBetsViewModel(query, null, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, string.Empty, string.Empty);

            // Act
            var actualResetLastTransId = viewModel.ResetLastTransId;

            // Assert
            Assert.Equal(expectedResetLastTransId, actualResetLastTransId);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        public void BetType_Always_ReturnsCorrectValue(int betType, int expectedBetType)
        {
            // Arrange
            var query = new LastBetsQuery(user)
            {
                BetType = betType
            };

            var viewModel = new LastBetsViewModel(query, null, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, string.Empty, string.Empty);

            // Act
            var actualBetType = viewModel.BetType;

            // Assert
            Assert.Equal(expectedBetType, actualBetType);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void IsSelectedBetType_Always_ReturnsCorrectValue(int betType, bool expectedIsSelectedBetType)
        {
            // Arrange
            var query = new LastBetsQuery(user)
            {
                BetType = 1
            };

            var viewModel = new LastBetsViewModel(query, null, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, string.Empty, string.Empty);

            // Act
            var actualIsSelectedBetType = viewModel.IsSelectedBetType(betType);

            // Assert
            Assert.Equal(expectedIsSelectedBetType, actualIsSelectedBetType);
        }

        [Theory]
        [InlineData(10, true)]
        [InlineData(100, false)]
        public void IsSelectedPageSize_Always_ReturnsCorrectValue(int pageSize, bool expectedIsSelectedPageSize)
        {
            // Arrange
            var query = new LastBetsQuery(user)
            {
                PageSize = 10
            };

            var viewModel = new LastBetsViewModel(query, null, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, string.Empty, string.Empty);

            // Act
            var actualIsSelectedPageSize = viewModel.IsSelectedPageSize(pageSize);

            // Assert
            Assert.Equal(expectedIsSelectedPageSize, actualIsSelectedPageSize);
        }
    }
}
