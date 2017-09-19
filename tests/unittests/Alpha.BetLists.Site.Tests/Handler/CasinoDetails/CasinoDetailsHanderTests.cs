namespace Alpha.BetLists.Site.Tests.Handler.CasinoDetails
{
    using BetLists.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetDetails.Casino.Services;
    using Fixtures;
    using Handlers.CasinoDetails;
    using NSubstitute;
    using Requests.CasioDetails;
    using Xunit;

    [Trait("Category", "CasinoDetails")]
    [Collection("BetListCollection")]
    public class CasinoDetailsHanderTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly IUser user;
        private readonly ICasinoDetailsService casinoDetailsService;
        private readonly CasinoDetailsHander handler;

        public CasinoDetailsHanderTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;
            user = betlistFixture.User;

            casinoDetailsService = Substitute.For<ICasinoDetailsService>();
            handler = new CasinoDetailsHander(
                casinoDetailsService,
                betlistFixture.HtmlBuilder,
                betlistFixture.TicketHelper,
                betlistFixture.MoniterService,
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
    }
}
