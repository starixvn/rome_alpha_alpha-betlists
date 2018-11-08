namespace Alpha.BetLists.Site.Tests.Handler.Parlay
{
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.Parlay;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Requests.Parlay;
    using Site.ViewModels.Parlay;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("ParlayBetListCollection")]
    public class ParlayHandlerTests
    {
        private readonly BetListFixture betListFixture;
        private readonly ParlayServiceFixture parlayServiceFixture;
        private readonly ParlayHandler parlayHandler;
        private readonly CompareLogic comparer;

        public ParlayHandlerTests(BetListFixture betListFixture, ParlayServiceFixture parlayServiceFixture)
        {
            this.betListFixture = betListFixture;
            this.parlayServiceFixture = parlayServiceFixture;

            parlayHandler = new ParlayHandler(
                this.parlayServiceFixture.ParlayService,
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper,
                betListFixture.MonitorService,
                betListFixture.IpService);

            comparer = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = { "HugeDataLabel" }
            });
        }

        [Fact]
        public void Handle_MixParlayQuery_CallGetParlayBetListService()
        {
            // Arrange
            var query = new MixParlayQuery(betListFixture.User);

            // Act
            parlayHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            parlayServiceFixture.ParlayService.Received().GetParlayBetList(
                Arg.Is<long>(t => t == query.TransId),
                Arg.Is<int>(l => l == query.ViewerId));
        }

        [Fact]
        public void Handle_MixParlayQuery_ReturnMixParlayViewModel()
        {
            // Arrange
            var query = new MixParlayQuery(betListFixture.User);
            var expectedViewModel = new MixParlayViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Act
            var actualViewModel = parlayHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(comparer.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public void Handle_ComboParlayQuery_CallGetParlayBetListService()
        {
            // Arrange
            var query = new ComboParlayQuery(betListFixture.User);

            // Act
            parlayHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            parlayServiceFixture.ParlayService.Received().GetParlayBetList(
                Arg.Is<long>(t => t == query.TransId),
                Arg.Is<int>(l => l == query.ViewerId));
        }

        [Fact]
        public void Handle_ComboParlayQuery_ReturnComboParlayViewModel()
        {
            // Arrange
            var query = new ComboParlayQuery(betListFixture.User);
            var expectedViewModel = new ComboParlayViewModel(
                query,
                new BetListData(BetListDataTest.Tickets, null),
                betListFixture.HtmlBuilder,
                betListFixture.TicketHelper);

            // Act
            var actualViewModel = parlayHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(comparer.Compare(expectedViewModel, actualViewModel).AreEqual);
        }

        [Fact]
        public void Handle_ComboParlayDetailQuery_CallGetComboParlayDetailService()
        {
            // Arrange
            var query = new ComboParlayDetailQuery(betListFixture.User);

            // Act
            parlayHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            parlayServiceFixture.ParlayService.Received().GetComboParlayDetail(
                Arg.Is<long>(t => t == query.TransId),
                Arg.Is<long>(r => r == query.RefNo),
                Arg.Is<long>(b => b == query.BetId),
                Arg.Is<int>(c => c == query.CustId),
                Arg.Is<string>(l => l == query.Language));
        }

        [Fact]
        public void Handle_ComboParlayDetailQuery_ReturnComboParlayDetailViewModel()
        {
            // Arrange
            var query = new ComboParlayDetailQuery(betListFixture.User);
            var expectedViewModel = new ComboParlayDetailViewModel(
                query,
                BetListDataTest.ComboParlayDetailData,
                betListFixture.TicketHelper);

            // Act
            var actualViewModel = parlayHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(comparer.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}