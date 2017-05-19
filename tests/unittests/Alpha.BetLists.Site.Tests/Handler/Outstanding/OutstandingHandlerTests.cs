namespace Alpha.BetLists.Site.Tests.Handler.Outstanding
{
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.Outstanding;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.Outstanding;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "Outstanding")]
    [Collection("BetListCollection")]
    public class OutstandingHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly OutstandingHandler outstandingHandler;
        private readonly OutstandingService outstandingService;
        private readonly ProductService productService;
        private readonly ForeCastService foreCastService;
        private readonly OutstandingQuery query;
        private readonly CompareLogic compareLogic;

        public OutstandingHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            outstandingService = Substitute.For<OutstandingService>();
            outstandingService.GetBetList(Arg.Any<OutstandingParam>()).Returns(BetListDataTest.BetListData);

            productService = Substitute.For<ProductService>();
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>()).Returns(BetListDataTest.ProductIds);

            foreCastService = Substitute.For<ForeCastService>();
            foreCastService.GetBetTypeIdsSupportScoreMap().Returns(BetListDataTest.BettypeIds);

            query = new OutstandingQuery(this.betlistFixture.User);

            outstandingHandler = new OutstandingHandler(
                outstandingService,
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
            outstandingHandler.Handle(query).GetAwaiter().GetResult();

            // Assert
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == expectedId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(BetListDataTest.Tickets)));
        }
    }
}