namespace Alpha.BetLists.Site.Tests.Fixtures
{
    using BetLists.Tests.TestData;
    using NSubstitute;
    using Services;

    public class ParlayServiceFixture
    {
        public ParlayServiceFixture()
        {
            ParlayService = Substitute.For<ParlayService>();
            ParlayService.GetParlayBetList(Arg.Any<long>(), Arg.Any<string>()).Returns(BetListDataTest.Tickets);
            ParlayService.GetComboParlayDetail(
                Arg.Any<long>(),
                Arg.Any<long>(),
                Arg.Any<long>(),
                Arg.Any<int>(),
                Arg.Any<string>()).Returns(BetListDataTest.ComboParlayDetailData);
        }

        public ParlayService ParlayService { get; }
    }
}