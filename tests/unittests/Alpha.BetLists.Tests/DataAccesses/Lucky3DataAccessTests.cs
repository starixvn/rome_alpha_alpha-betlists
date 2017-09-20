namespace Alpha.BetLists.Tests.DataAccesses
{
    using Alpha.BetLists.Criteria.Lucky3;
    using Alpha.BetLists.DataAccesses;
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.DataAccesses;
    using Fanex.BetListDetail.Lucky3.Models;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    public class Lucky3DataAccessTests
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ILucky3DataAccess dataAccess;

        public Lucky3DataAccessTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            dataAccess = new Lucky3DataAccess(dynamicRepository);
        }

        [Fact]
        public void GetLucky3Detail_Always_CallFetchOfDynamicRepository()
        {
            //// Arrange
            var parameter = Substitute.For<IGetLucky3DetailParameter>();

            //// Act
            var act = dataAccess.GetLucky3Detail(parameter);

            //// Assert
            dynamicRepository.Received().Fetch<Lucky3DetailModel>(Arg.Any<GetLucky3DetailCriteria>());
        }
    }
}
