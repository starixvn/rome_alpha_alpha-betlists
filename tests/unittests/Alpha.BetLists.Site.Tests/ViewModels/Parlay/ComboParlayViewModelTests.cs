namespace Alpha.BetLists.Site.Tests.ViewModels.Parlay
{
    using System.Collections.Generic;
    using BetLists.Models;
    using Fixtures;
    using NSubstitute;
    using Requests.Parlay;
    using Site.ViewModels.Parlay;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class ComboParlayViewModelTests
    {
        private readonly BetListFixture betListFixture;
        private readonly ComboParlayQuery query;

        public ComboParlayViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            query = new ComboParlayQuery(betListFixture.User);
        }

        private static List<string> ComboParlayOfferNames
            => new List<string>
            {
                "Double", "Treble"
            };

        private static BetListData ComboParlayData => new BetListData(ComboParlayTickets, null);

        private static List<TicketDto> ComboParlayTickets
            => new List<TicketDto>
            {
                new TicketDto { Currency = "UUS", Stake = 10, MatchId = 1 },
                new TicketDto { Currency = "UUS", Stake = 20, MatchId = 2 }
            };

        [Fact]
        public void UnitStakeByOfferNames_IsComboParlayTicket_ReturnUnitStakeOfferNames()
        {
            // Arrange
            query.Handicap1 = 1;
            query.BetCheck = "BetCheck";
            betListFixture
                .TicketHelper
                .GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2)
                .Returns(ComboParlayOfferNames);
            var expectedUnitStakeOfferNames = new Dictionary<string, string>
            {
                { "Double", "UUS 10" },
                { "Treble", "UUS 20" }
            };

            // Act
            var viewModel = new ComboParlayViewModel(query, ComboParlayData, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            // Assert
            Assert.Equal(expectedUnitStakeOfferNames, viewModel.UnitStakeByOfferNames);
        }

        [Fact]
        public void UnitStakeByOfferNames_IsSystemParlayTicket_ReturnUnitStakeOfferNames()
        {
            // Arrange
            query.Handicap1 = 0;
            betListFixture
                .TicketHelper
                .GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2)
                .Returns(ComboParlayOfferNames);
            betListFixture
                .TicketHelper
                .GetCombinationMPOfferName(query.BetTeam)
                .Returns("Treble");

            var expectedUnitStakeOfferNames = new Dictionary<string, string>();

            // Act
            var viewModel = new ComboParlayViewModel(query, ComboParlayData, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            // Assert
            Assert.Equal(expectedUnitStakeOfferNames, viewModel.UnitStakeByOfferNames);
        }

        [Fact]
        public void UnitStakeByOfferNames_HasInvalidData_ReturnEmptyList()
        {
            // Arrange
            query.Handicap1 = 0;
            betListFixture
                .TicketHelper
                .GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2)
                .Returns(ComboParlayOfferNames);
            var expectedUnitStakeOfferNames = new Dictionary<string, string>();
            var emptyComboParlayData = new BetListData(new List<TicketDto>(), null);

            // Act
            var viewModel = new ComboParlayViewModel(query, emptyComboParlayData, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            // Assert
            Assert.Equal(expectedUnitStakeOfferNames, viewModel.UnitStakeByOfferNames);
        }
    }
}