namespace Alpha.BetLists.Site.Tests.Handler.MemberBetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Site.ViewModels.Components;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.MemberBetList;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.MemberBetList;
    using Services;
    using Site.ViewModels.MemberBetList;
    using Xunit;

    [Trait("Category", "MemberBetList")]
    [Collection("BetListCollection")]
    public class MemberBetListHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly MemberBetListHandler memberBetListHandler;
        private readonly MemberBetListService memberBetListService;
        private readonly ProductService productService;
        private readonly ForecastService forecastService;
        private readonly CompareLogic compareLogic;

        public MemberBetListHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            memberBetListService = Substitute.For<MemberBetListService>();
            memberBetListService.GetBetList(Arg.Any<MemberBetListParam>()).Returns(BetListDataTest.BetListData);

            productService = Substitute.For<ProductService>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<IEnumerable<int>>(), false).Returns(BetListDataTest.ProductIds);

            forecastService = Substitute.For<ForecastService>();
            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            memberBetListHandler = new MemberBetListHandler(
                memberBetListService,
                productService,
                forecastService,
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
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
            var query = new MemberBetListQuery(betlistFixture.User);
            query.CustId = 1;
            query.ProductIds = "1";
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.LimitedDateToShowInfo = DateTime.MinValue;
            query.Language = "en";
            query.CustName = "custName";

            var validUserSelectedProductIds = Enumerable.Empty<int>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<IEnumerable<int>>(), false).Returns(validUserSelectedProductIds);

            forecastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            var memberBetListParam = new MemberBetListParam(
                1,
                validUserSelectedProductIds,
                query.FromDate,
                query.ToDate,
                query.LimitedDateToShowInfo,
                query.Language,
                query.CustName);

            var expectedMemberBetListViewModel = new MemberBetListViewModel(
                query,
                memberBetListParam,
                BetListDataTest.BetListData,
                Enumerable.Empty<GroupProductViewModel>(),
                BetListDataTest.BettypeIds,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualMemberBetListViewModel = memberBetListHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.True(compareLogic.Compare(actualMemberBetListViewModel, expectedMemberBetListViewModel).AreEqual);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Handle_IsEnabledJackpot_ShowContributionAmountRoundNoteReturnsCorrectValue(bool isEnabledJackpot)
        {
            // Arrange
            var query = new MemberBetListQuery(betlistFixture.User);
            query.IsEnabledJackpot = isEnabledJackpot;
            query.CustId = 1;
            query.ProductIds = "1";
            query.FromDate = DateTime.Today;
            query.ToDate = DateTime.Today;
            query.Language = "en";
            query.CustName = "custName";

            // Act
            var viewModel = memberBetListHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(isEnabledJackpot, viewModel.ShowContributionAmountRoundNote);
        }
    }
}