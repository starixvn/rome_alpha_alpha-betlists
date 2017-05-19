namespace Alpha.BetLists.Site.Tests.Handler.RunningBetList
{
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.RunningBetList;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Requests.RunningBetList;
    using Services;
    using System.Collections.Generic;
    using System.Linq;
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
    }
}