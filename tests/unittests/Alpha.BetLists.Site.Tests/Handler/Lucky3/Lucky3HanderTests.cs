namespace Alpha.BetLists.Site.Tests.Handler.Lucky3
{
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Site.Handlers;
    using Alpha.BetLists.Site.Requests.Lucky3;
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.Services;
    using Fanex.BetListDetail.Lucky3.ViewModels;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Ploeh.AutoFixture;
    using Xunit;

    public class Lucky3HanderTests
    {
        private readonly ILucky3Service service;
        private readonly Lucky3Handler handler;
        private readonly IUser user;
        private readonly IFixture fixture;
        private readonly CompareLogic comparer;

        public Lucky3HanderTests()
        {
            fixture = new Fixture();
            service = Substitute.For<ILucky3Service>();
            handler = new Lucky3Handler(service);
            user = Substitute.For<IUser>();
            user.LevelId.Returns(UserLevel.Agent.Value);
            comparer = new CompareLogic();
        }

        [Fact]
        public void Handle_AlwaysCallService_CorrectParameter()
        {
            //// Arrange            
            var message = new Lucky3DetailQuery(user);

            //// Act
            var act = handler.Handle(message);

            //// Assert
            service.Received().GetBetListDetail(Arg.Is<IGetLucky3DetailParameter>(i => i.CustomerId == message.CustId && i.TransactionId == message.TransactionId));
        }

        [Fact]
        public void Handle_Alway_ReturnViewModelType()
        {
            //// Arrange
            var message = new Lucky3DetailQuery(user);
            var viewModel = fixture.Create<Lucky3DetailViewModel>();
            service.GetBetListDetail(Arg.Any<IGetLucky3DetailParameter>()).Returns(viewModel);

            // Act
            var act = handler.Handle(message);

            //// Assert
            Assert.IsType(typeof(Lucky3DetailViewModel), act);
        }
    }
}
