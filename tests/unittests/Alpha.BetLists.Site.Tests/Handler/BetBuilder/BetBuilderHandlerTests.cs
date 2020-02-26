using Alpha.BetLists.Site.Handlers;
using Fanex.BetDetails.BetBuilder.Services;
using NSubstitute;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.BetBuilder
{
    [Trait("Category", "BetBuilder")]
    public class BetBuilderHandlerTests
    {
        private readonly BetBuilderHandler handler;
        private readonly IBetBuilderService service;

        public BetBuilderHandlerTests()
        {
            service = Substitute.For<IBetBuilderService>();
            handler = new BetBuilderHandler(service);
        }

        [Fact]
        public void BetBuilderService_Always_ReceiveCallFromHandler()
        {
            handler.Handle(new Requests.BetBuilder.BetBuilderDetailQuery());
            service.Received();
        }
    }
}
