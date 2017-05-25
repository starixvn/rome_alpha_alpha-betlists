namespace Alpha.BetLists.Site.Tests.Handler.MemberBetList
{
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.MemberBetList;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.MemberBetList;
    using Services;
    using Site.ViewModels.MemberBetList;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    [Collection("BetListCollection")]
    public class MemberBetListHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly MemberBetListHandler memberBetListHandler;
        private readonly MemberBetListService memberBetListService;
        private readonly ProductService productService;
        private readonly ForeCastService foreCastService;
        private readonly MemberBetListQuery query;
        private readonly CompareLogic compareLogic;

        public MemberBetListHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            memberBetListService = Substitute.For<MemberBetListService>();
            memberBetListService.GetBetList(Arg.Any<MemberBetListParam>()).Returns(BetListDataTest.BetListData);

            productService = Substitute.For<ProductService>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(BetListDataTest.ProductIds);

            foreCastService = Substitute.For<ForeCastService>();
            foreCastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            query = new MemberBetListQuery(this.betlistFixture.User);

            memberBetListHandler = new MemberBetListHandler(
                memberBetListService,
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

        [Fact]
        public void Handle_AnyProductIds_ReturnCorrectViewModel()
        {
            // Arrange
            query.CustId = 1;
            query.ProductIds = "1";
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.Language = "en";
            query.CustName = "custName";

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(validUserSelectedProductIds);

            foreCastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            var memberBetListParam = new MemberBetListParam(
                1,
                validUserSelectedProductIds,
                query.FromDate,
                query.ToDate,
                query.Language,
                query.CustName);

            var expectedMemberBetListViewModel = new MemberBetListViewModel(
                query,
                memberBetListParam,
                BetListDataTest.BetListData,
                Enumerable.Empty<SelectListItem>(),
                BetListDataTest.BettypeIds,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualMemberBetListViewModel = memberBetListHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualMemberBetListViewModel, expectedMemberBetListViewModel).AreEqual);
        }
    }
}