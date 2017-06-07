namespace Alpha.BetLists.Site.Tests.ViewModels.MemberBetList
{
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Helpers;
    using NSubstitute;
    using Params;
    using Requests.MemberBetList;
    using Site.ViewModels.MemberBetList;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Utils;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    [Collection("BetListCollection")]
    public class MemberBetListViewModelTests
    {
        private readonly IUser user;
        private readonly BetListFixture betlistFixture;
        private MemberBetListQuery query;
        private MemberBetListParam param;

        public MemberBetListViewModelTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            user = betlistFixture.User;

            SystemTime.Today = DefaultVariables.CurrentDate;

            query = new MemberBetListQuery(user)
            {
                CustId = 1,
                CustName = "CustName"
            };
            param = new MemberBetListParam(
                query.CustId,
                Enumerable.Empty<int>(),
                query.FromDate,
                query.ToDate,
                query.Language,
                query.CustName);
        }

        [Fact]
        public void Title_Always_ReturnsCorectValue()
        {
            // Arrange
            var viewModel = new MemberBetListViewModel(
                query,
                param,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                Enumerable.Empty<int>(),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            var expectedTitle = "Bet List - CustName";

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void ViewDataInstructionMessage_Always_ReturnsCorrectValue()
        {
            // Arrange
            var viewModel = new MemberBetListViewModel(
                query,
                param,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                Enumerable.Empty<int>(),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            var expectedMessage = "You can view the report data from 02/01/2017.";

            // Act
            var actualMessage = viewModel.ViewDataInstructionMessage;

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void BuildBetListLink_Always_ReturnsCorrectLink()
        {
            // Arrange
            var currentUrl = "CurrentUrl";
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            var viewModel = new MemberBetListViewModel(
                query,
                param,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                Enumerable.Empty<int>(),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            viewModel.BuildRunningBetListLink(currentUrl);

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                currentUrl,
                Arg.Is<IDictionary<string, object>>(
                    routeValues =>
                    (int)routeValues["CustId"] == 1
                    && (string)routeValues["CustName"] == "CustName"
                    && (string)routeValues["FromDate"] == "05/18/2017"
                    && (string)routeValues["ToDate"] == "05/25/2017"
                    && (string)routeValues["ProductIds"] == string.Empty));
        }

        [Theory]
        [InlineData(BetStatus.Running, 1)]
        [InlineData(BetStatus.Refund, 1)]
        [InlineData(BetStatus.Running, 4)]
        [InlineData(BetStatus.Waiting, 4)]
        public void CanViewScoreMap_Always_ReturnsCorrectValue(string ticketStatus, int bettypeId)
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = bettypeId,
                Status = ticketStatus
            };

            var supportScoreMapBettypeIds = new List<int> { 1, 2, 3 };
            var expectedCanViewScoreMap = ticketStatus.Equals(BetStatus.Running) && supportScoreMapBettypeIds.Contains(bettypeId);

            var viewModel = new MemberBetListViewModel(
                query,
                param,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                supportScoreMapBettypeIds,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualCanViewScoreMap = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.Equal(expectedCanViewScoreMap, actualCanViewScoreMap);
        }

        [Fact]
        public void GetDailyStatementLink_Always_ReturnsCorrectLink()
        {
            // Arrange
            UrlHelperTestSetUp.SetUp();

            // Act
            MemberBetListViewModel.GetDailyStatementLink(1, "CustName", "1,2,3");

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewReportsPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValues =>
                    (int)routeValues["CustId"] == 1
                    && (string)routeValues["CustName"] == "CustName"
                    && (int)routeValues["CustLevelId"] == 1
                    && (string)routeValues["ProductIds"] == "1,2,3"));
        }
    }
}