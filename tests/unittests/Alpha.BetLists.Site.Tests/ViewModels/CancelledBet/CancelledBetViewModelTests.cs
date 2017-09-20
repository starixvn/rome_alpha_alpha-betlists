namespace Alpha.BetLists.Site.Tests.ViewModels.CancelledBet
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Models.CancelledBet;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.BetLists.Site.Tests.Fixtures;
    using Alpha.BetLists.Site.ViewModels.CancelledBet;
    using Alpha.BetLists.Tests.TestData;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "CancelledBet")]
    [Collection("BetListCollection")]
    public class CancelledBetViewModelTests
    {
        private readonly BetListFixture betListFixture;
        private readonly UserBetCount userBetCount1;
        private readonly IEnumerable<UserBetCount> userBetCounts;

        public CancelledBetViewModelTests(BetListFixture betListFixture)
        {
            this.betListFixture = betListFixture;
            userBetCount1 = new UserBetCount(263, "razor", 2);
            userBetCounts = new List<UserBetCount> { userBetCount1 };
        }

        [Fact]
        public void MasterList_ViewerIsSuper_ReturnMasterList()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Masters = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.MasterList;

            //// Assert
            Assert.Equal(1, act.Count());
            Assert.Equal("263", act.First().Value);
            Assert.Equal("razor (2 bets)", act.First().Text);
        }

        [Fact]
        public void AgentList_ViewerIsSuper_ReturnAgentList()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Agents = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.AgentList;

            //// Assert
            Assert.Equal(2, act.Count());
            Assert.Equal("All Agent", act.First().Text);
            Assert.Equal("0", act.First().Value);

            Assert.Equal("263", act.ElementAt(1).Value);
            Assert.Equal("razor (2 bets)", act.ElementAt(1).Text);
        }

        [Fact]
        public void MemberList_ViewerIsSuper_ReturnMemberList()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Members = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.MemberList;

            //// Assert
            Assert.Equal(2, act.Count());
            Assert.Equal("All Member", act.First().Text);
            Assert.Equal("0", act.First().Value);

            Assert.Equal("263", act.ElementAt(1).Value);
            Assert.Equal("razor (2 bets)", act.ElementAt(1).Text);
        }

        [Fact]
        public void AgentList_ViewerIsMaster_ReturnAgentNotIncludeAllAgentText()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Master.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Agents = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.AgentList;

            //// Assert
            Assert.Equal(1, act.Count());
            Assert.Equal("263", act.First().Value);
            Assert.Equal("razor (2 bets)", act.First().Text);
        }

        [Fact]
        public void MemberList_ViewerIsMaster_ReturnMemberList()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Master.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Members = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.MemberList;

            //// Assert
            Assert.Equal(2, act.Count());
            Assert.Equal("All Member", act.First().Text);
            Assert.Equal("0", act.First().Value);

            Assert.Equal("263", act.ElementAt(1).Value);
            Assert.Equal("razor (2 bets)", act.ElementAt(1).Text);
        }

        [Fact]
        public void MembeList_ViewerIsAgent_ReturnMemberListNoIncludeAllMemberText()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Agent.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Members = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.MemberList;

            //// Assert
            Assert.Equal("263", act.First().Value);
            Assert.Equal("razor (2 bets)", act.First().Text);
        }

        [Fact]
        public void IsShowMasterList_ViewerIsSuperAndHasTickets_ReturnTrue()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Masters = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.IsShowMasterList;

            //// Assert
            Assert.True(act);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void IsShowMasterList_ViewerIsNotSuper_ReturnFalse(byte levelNotSuper)
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(levelNotSuper);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Masters = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.IsShowMasterList;

            //// Assert
            Assert.False(act);
        }

        [Fact]
        public void IsShowAgentList_ViewerIsMasterAndHasTickets_ReturnTrue()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Agents = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.IsShowAgentList;

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsShowAgentList_ViewerIsSuperAndHasTickets_ReturnTrue()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Agents = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.IsShowAgentList;

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsShowAgentList_ViewerIsAgentAndHasTickets_ReturnFalse()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Agent.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel();
            filter.Agents = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.IsShowAgentList;

            //// Assert
            Assert.False(act);
        }

        [Fact]
        public void IsShowMemberList_MemberListHasData_ReturnTrue()
        {
            //// Arrange            
            var query = new CancelledBetQuery(betListFixture.User);
            var filter = new FilterCancelledBetModel()
            {
                Members = userBetCounts,
                MemberId = 263
            };
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.IsShowMemberList;

            //// Assert
            Assert.True(act);
        }

        [Fact]
        public void IsShowMemberList_SelectedItemInAgentListIsAll_ReturnFalse()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User);
            query.AgentId = 0;
            var filter = new FilterCancelledBetModel();
            filter.Agents = userBetCounts;
            filter.Members = userBetCounts;
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.IsShowMemberList;

            //// Assert
            Assert.False(act);
        }

        [Fact]
        public void MemberList_SelectTheItem_TheSelectedOfItemIsTrue()
        {
            //// Arrange            
            var query = new CancelledBetQuery(betListFixture.User)
            {
                MemberId = 263
            };
            var filter = new FilterCancelledBetModel()
            {
                Members = userBetCounts
            };

            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.MemberList;

            //// Assert            
            Assert.True(act.ElementAt(1).Selected);
        }

        [Fact]
        public void AgentList_SelectTheItem_TheSelectedOfItemIsTrue()
        {
            //// Arrange            
            var query = new CancelledBetQuery(betListFixture.User)
            {
                AgentId = 263
            };
            var filter = new FilterCancelledBetModel()
            {
                Agents = userBetCounts
            };
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.AgentList;

            //// Assert            
            Assert.True(act.ElementAt(1).Selected);
        }

        [Fact]
        public void MasterList_SelectTheItem_TheSelectedOfItemIsTrue()
        {
            //// Arrange
            betListFixture.User.LevelId.Returns(UserLevel.Super.Value);
            var query = new CancelledBetQuery(betListFixture.User)
            {
                MasterId = 263
            };
            var filter = new FilterCancelledBetModel()
            {
                Masters = userBetCounts
            };
            var model = new CancelledBetModel(filter, BetListDataTest.BetListData);
            var viewModel = new CancelledBetViewModel(query, model, betListFixture.HtmlBuilder, betListFixture.TicketHelper);

            //// Act
            var act = viewModel.MasterList;

            //// Assert            
            Assert.True(act.First().Selected);
        }
    }
}
