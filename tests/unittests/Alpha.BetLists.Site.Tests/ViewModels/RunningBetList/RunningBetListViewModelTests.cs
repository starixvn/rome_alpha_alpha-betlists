namespace Alpha.BetLists.Site.Tests.ViewModels.RunningBetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using BetLists.Models;
    using BetLists.Tests.Constants;
    using BetLists.Tests.TestData;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Requests.RunningBetList;
    using Site.ViewModels.RunningBetList;
    using Utils;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    [Collection("BetListCollection")]
    public class RunningBetListViewModelTests
    {
        private readonly IUser user;
        private readonly BetListFixture betlistFixture;
        private RunningBetListQuery query;
        private RunningBetListParam param;

        public RunningBetListViewModelTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            user = betlistFixture.User;

            SystemTime.Today = DefaultVariables.CurrentDate;

            query = new RunningBetListQuery(user)
            {
                CustId = 1,
                CustName = "CustName"
            };
            param = new RunningBetListParam(
                query.CustId,
                Enumerable.Empty<int>(),
                query.FromDate,
                query.ToDate,
                query.GetAll,
                query.Language,
                query.CustName);
        }

        [Fact]
        public void Title_Always_ReturnsCorectValue()
        {
            // Arrange
            var viewModel = new RunningBetListViewModel(
                query,
                param,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                Enumerable.Empty<int>(),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            var expectedTitle = "Bet List (Running) - CustName";

            // Act
            var actualTitle = viewModel.Title;

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void ViewDataInstructionMessage_Always_ReturnsCorrectValue()
        {
            // Arrange
            var viewModel = new RunningBetListViewModel(
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

            var viewModel = new RunningBetListViewModel(
                query,
                param,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                Enumerable.Empty<int>(),
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            viewModel.BuildBetListLink(currentUrl);

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                currentUrl,
                Arg.Is<IDictionary<string, object>>(
                    routeValues =>
                    (int)routeValues["CustId"] == 1
                    && (string)routeValues["CustName"] == "CustName"
                    && (string)routeValues["FromDate"] == "05/24/2017"
                    && (string)routeValues["ToDate"] == "05/25/2017"
                    && (string)routeValues["ProductIds"] == string.Empty
                    && (int)routeValues["UserSelectedDateRangeOption"] == 7));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        public void CanViewScoreMap_Always_ReturnsCorrectValue(int bettypeId)
        {
            // Arrange
            var ticket = new TicketDto
            {
                BetTypeId = bettypeId
            };

            var supportScoreMapBetTypeIds = new List<int> { 1, 2, 3 };
            var expectedCanViewScoreMap = supportScoreMapBetTypeIds.Contains(bettypeId);

            var viewModel = new RunningBetListViewModel(
                query,
                param,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                supportScoreMapBetTypeIds,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualCanViewScoreMap = viewModel.CanViewScoreMap(ticket);

            // Assert
            Assert.Equal(expectedCanViewScoreMap, actualCanViewScoreMap);
        }
    }
}