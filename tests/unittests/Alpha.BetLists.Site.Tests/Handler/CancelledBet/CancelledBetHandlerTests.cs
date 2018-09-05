namespace Alpha.BetLists.Site.Tests.Handler.CancelledBet
{
    using System;
    using System.Collections.Generic;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Handlers.CancelledBet;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.Utilities.IP;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using SmartPunter.Services;
    using Xunit;

    [Trait("Category", "CancelledBet")]
    public class CancelledBetHandlerTests
    {
        private readonly CancelledBetService service;
        private readonly ProductService productService;
        private readonly ReportDateService reportDateService;
        private readonly ISmartPunterBetListMonitor monitorService;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;
        private readonly IPService ipService;
        private readonly IUser user;
        private CancelledBetHandler handler;

        public CancelledBetHandlerTests()
        {
            service = Substitute.For<CancelledBetService>();
            productService = Substitute.For<ProductService>();
            reportDateService = Substitute.For<ReportDateService>();
            monitorService = Substitute.For<ISmartPunterBetListMonitor>();
            ipService = Substitute.For<IPService>();
            ipService.IsVNIP(Arg.Any<string>()).Returns(true);
            builder = Substitute.For<IBetListHTMLBuilder>();
            ticketHelper = Substitute.For<ITicketHelper>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            handler = new CancelledBetHandler(service, productService, reportDateService, builder, ticketHelper, monitorService, ipService);
        }

        [Fact]
        public void Handle_Always_Call3Service()
        {
            //// Arrange
            var productIds = new List<int> { 1, 2, 3 };
            var query = new CancelledBetQuery(user);
            productService.GetProductsHaveSupportCancelledBet(Arg.Any<int>(), Arg.Any<DateTime>()).Returns(productIds);

            //// Act
            var act = handler.Handle(query);

            //// Assert
            productService.Received().GetProductsHaveSupportCancelledBet(Arg.Any<int>(), Arg.Any<DateTime>());
            productService.Received().GetProductsForCustomer(Arg.Any<int>(), Arg.Any<DateTime>());
            service.Received().GetCancelledBet(Arg.Is<CancelledBetParam>(i => i.ProductIds == productIds));
        }

    }
}
