namespace Alpha.BetLists.Site.Tests.ViewModels.Parlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BetLists.Models;
    using Fanex.BetList.Core.Entities;
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

        public ComboParlayViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
        }

        private List<string> ComboParlayOfferNames
            => new List<string>
            {
                "Double", "Treble"
            };

        private BetListData ComboParlayData => new BetListData(ComboParlayTickets, null);

        private List<TicketDto> ComboParlayTickets
            => new List<TicketDto>
            {
                new TicketDto { Currency = "UUS", Stake = 10, MatchId = 1 },
                new TicketDto { Currency = "UUS", Stake = 20, MatchId = 2 }
            };

        [Fact]
        public void UnitStakeByOfferNames_IsComboParlayTicket()
        {
            // Arrange
            var query = new ComboParlayQuery(betListFixture.User);
            query.Handicap1 = 1;
            betListFixture.TicketHelper.GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2)
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
        public void UnitStakeByOfferNames_IsSystemParlayTicket()
        {
            // Arrange
            var query = new ComboParlayQuery(betListFixture.User);
            query.Handicap1 = 0;
            betListFixture.TicketHelper.GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2)
                .Returns(ComboParlayOfferNames);
            var expectedUnitStakeOfferNames = new Dictionary<string, string>
            {
                { "Double", "UUS 10" },
                { "Treble", "UUS 10" }
            };

            // Act
            var viewModel = new ComboParlayViewModel(query, ComboParlayData, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            // Assert
            Assert.Equal(expectedUnitStakeOfferNames, viewModel.UnitStakeByOfferNames);
        }
    }
}