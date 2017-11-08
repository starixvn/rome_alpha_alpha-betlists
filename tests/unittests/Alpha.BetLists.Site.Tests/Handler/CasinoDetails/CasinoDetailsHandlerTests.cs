namespace Alpha.BetLists.Site.Tests.Handler.CasinoDetails
{
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetDetails.Casino.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.Services;
    using Fanex.BetDetails.Casino.ViewModels;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using Handlers.CasinoDetails;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Requests.CasinoDetails;
    using Xunit;

    [Trait("Category", "CasinoDetails")]
    [Collection("BetListCollection")]
    public class CasinoDetailsHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly IUser user;
        private readonly ICasinoDetailsService casinoDetailsService;
        private readonly CasinoDetailsHandler handler;

        public CasinoDetailsHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            user = betlistFixture.User;

            casinoDetailsService = Substitute.For<ICasinoDetailsService>();
            handler = new CasinoDetailsHandler(
                casinoDetailsService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MonitorService,
                betlistFixture.IpService);
        }

        [Fact]
        public void Handle_Always_CallsGetBetListFromService()
        {
            // Arrange
            var message = new CasinoDetailsQuery(user);

            // Act
            handler.Handle(message);

            // Assert
            casinoDetailsService.Received().GetCasinoDetails(
                Arg.Is<CasinoDetailsParam>(
                    p => p.RefNo == message.RefNo
                    && p.CustId == message.CustId
                    && p.TransType == message.TransType
                    && p.ProductId == message.ProductId
                    && p.PageIndex == message.PageIndex
                    && p.PageSize == message.PageSize));
        }

        [Fact]
        public void Handle_Always_ReturnsCasinoBetDetailsViewModel()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var message = new CasinoDetailsQuery(user);
            var param = new CasinoDetailsParam(
                message.RefNo,
                message.CustId,
                message.TransType,
                message.ProductId,
                message.PageIndex,
                message.PageSize);

            var pagedData = new PagedData<ITicket>(BetListDataTest.Tickets, 0, 0, 0);
            casinoDetailsService.GetCasinoDetails(Arg.Any<CasinoDetailsParam>()).Returns(pagedData);

            var expectedViewModel = new CasinoDetailsViewModel(
                message.SportName,
                message.ViewerId,
                message.ViewerLevelId,
                param,
                pagedData,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper);

            // Act
            var actualViewModel = handler.Handle(message);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}
