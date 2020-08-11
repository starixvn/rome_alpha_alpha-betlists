namespace Alpha.BetLists.Site.Tests.Handler.CancelledBet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Params;
    using Alpha.BetLists.Services;
    using Alpha.BetLists.Site.Handlers.CancelledBet;
    using Alpha.BetLists.Site.Requests.CancelledBet;
    using Alpha.Utilities.IP;
    using BetLists.Tests.TestData;
    using Constants;
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

            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);
        }

        [Fact]
        public void Handle_Always_Call3Service()
        {
            //// Arrange            
            var query = new CancelledBetQuery(user);
            var products = BetListDataTest.Products;
            productService.GetProductsHaveSupportCancelledBet(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<DateTime>()).Returns(products);

            //// Act
            var act = handler.Handle(query);

            //// Assert
            productService.Received().GetProductsHaveSupportCancelledBet(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<DateTime>());
            service.Received().GetCancelledBet(Arg.Any<CancelledBetParam>());
        }

    }
}
