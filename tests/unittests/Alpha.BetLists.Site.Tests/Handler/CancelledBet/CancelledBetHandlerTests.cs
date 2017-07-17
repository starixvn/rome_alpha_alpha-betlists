using Alpha.BetLists.Models;
using Alpha.BetLists.Models.CancelledBet;
using Alpha.BetLists.Params;
using Alpha.BetLists.Services;
using Alpha.BetLists.Site.Handlers.CancelledBet;
using Alpha.BetLists.Site.Requests.CancelledBet;
using Alpha.BetLists.Tests.TestData;
using Alpha.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Alpha.BetLists.Site.Tests.Handler.CancelledBet
{
    [Trait("Category", "CancelledBet")]
    public class CancelledBetHandlerTests
    {
        private readonly CancelledBetService service;
        private readonly IBetListHTMLBuilder builder;
        private readonly ITicketHelper ticketHelper;
        private readonly IPService ipService;
        private readonly IUser user;
        private CancelledBetHandler handler;

        public CancelledBetHandlerTests()
        {
            service = Substitute.For<CancelledBetService>();
            ipService = Substitute.For<IPService>();
            ipService.IsVNIP(Arg.Any<string>()).Returns(true);
            builder = Substitute.For<IBetListHTMLBuilder>();
            ticketHelper = Substitute.For<ITicketHelper>();
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            handler = new CancelledBetHandler(service, builder, ticketHelper, ipService);
        }

        [Fact]
        public void Handle_Always_CallGetCancelledBetFromService()
        {
            //// Arrange
            var message = new CancelledBetQuery(user);
            var model = new CancelledBetModel(new FilterCancelledBetModel(), BetListDataTest.BetListData);
            service.GetCancelledBet(Arg.Any<CancelledBetParam>()).Returns(model);

            //// Act
            handler.Handle(message).GetAwaiter().GetResult();

            //// Assert
            service.Received().GetCancelledBet(Arg.Any<CancelledBetParam>());
        }
    }
}
