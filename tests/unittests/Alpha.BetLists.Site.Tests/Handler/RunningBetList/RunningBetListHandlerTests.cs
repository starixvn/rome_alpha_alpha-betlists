namespace Alpha.BetLists.Site.Tests.Handler.RunningBetList
{
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.RunningBetList;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.RunningBetList;
    using Services;
    using Site.ViewModels.RunningBetList;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Xunit;

    [Trait("Category", "RunningBetList")]
    [Collection("BetListCollection")]
    public class RunningBetListHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly RunningBetListHandler runningBetListHandler;
        private readonly RunningBetListService runningBetListService;
        private readonly ProductService productService;
        private readonly ForeCastService foreCastService;
        private readonly RunningBetListQuery query;
        private readonly CompareLogic compareLogic;

        public RunningBetListHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            runningBetListService = Substitute.For<RunningBetListService>();
            runningBetListService.GetBetList(Arg.Any<RunningBetListParam>()).Returns(BetListDataTest.BetListData);

            productService = Substitute.For<ProductService>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(BetListDataTest.ProductIds);

            foreCastService = Substitute.For<ForeCastService>();
            foreCastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            query = new RunningBetListQuery(this.betlistFixture.User);

            runningBetListHandler = new RunningBetListHandler(
                runningBetListService,
                productService,
                foreCastService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MoniterService,
                this.betlistFixture.IpService);

            compareLogic = new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "ResultsPath", "OldMainPath" }
            });
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(0, 10)]
        public void Handle_Always_CallWriteLog(int custId, int expectedId)
        {
            // Arrange
            query.CustId = custId;

            // Act
            runningBetListHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }

        [Fact]
        public void Handle_AnyProductIds_ReturnCorrectViewModel()
        {
            // Arrange
            query.CustId = 1;
            query.ProductIds = "1";
            query.GetAll = true;
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.Language = "en";
            query.CustName = "custName";

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            foreCastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            var runningBetListParram = new RunningBetListParam(
                1,
                validUserSelectedProductIds,
                query.FromDate,
                query.ToDate,
                query.GetAll,
                query.Language,
                query.CustName);

            var expectedRunningViewModel = new RunningBetListViewModel(
                query,
                runningBetListParram,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                BetListDataTest.BettypeIds,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualRunningViewModel = runningBetListHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualRunningViewModel, expectedRunningViewModel).AreEqual);
        }

        [Fact]
        public void BuildProductSelectListItems_NullProducts_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<Product> products = null;
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = runningBetListHandler.BuildProductSelectListItems(Enumerable.Empty<int>(), products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_EmptyProductList_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<Product> products = Enumerable.Empty<Product>();
            var userSelectedProductIds = Enumerable.Empty<int>();
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = runningBetListHandler.BuildProductSelectListItems(userSelectedProductIds, products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_HasDataProductList_ReturnsCorrectSelectListItem()
        {
            // Arrange
            Func<int, string> getProductDisplayName = (id) => $"Product {id}";

            IEnumerable<Product> products = new List<Product>
            {
                new Product(1, "Product 1", getProductDisplayName),
                new Product(2, "Product 2", getProductDisplayName),
                new Product(3, "Product 3", getProductDisplayName)
            };

            var userSelectedProductIds = new List<int> { 1, 3 };
            IEnumerable<SelectListItem> expectedSelectListItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Product 1",
                    Value = "1",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Product 2",
                    Value = "2",
                    Selected = false
                },
                new SelectListItem
                {
                    Text = "Product 3",
                    Value = "3",
                    Selected = true
                }
            };

            // Act
            var actualSelectListItems = runningBetListHandler.BuildProductSelectListItems(userSelectedProductIds, products).ToList();

            // Assert
            Assert.True(compareLogic.Compare(expectedSelectListItems, actualSelectListItems).AreEqual);
        }
    }
}