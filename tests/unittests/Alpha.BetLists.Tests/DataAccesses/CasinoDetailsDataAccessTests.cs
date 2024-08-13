namespace Alpha.BetLists.Tests.DataAccesses
{
    using System;
    using BetLists.Criteria.CasinoDetails;
    using BetLists.DataAccesses;
    using BetLists.Models;
    using Fanex.BetDetails.Casino.Models;
    using Fanex.BetDetails.Casino.Params;
    using Fanex.BetList.Core.Entities;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using TestData;
    using Xunit;

    [Trait("Category", "CasinoDetails")]
    [Collection("FanexDataCollection")]
    public class CasinoDetailsDataAccessTests
    {
        private readonly FanexDataFixture dataFixture;
        private readonly CasinoDetailsDataAccess dataAccess;

        public CasinoDetailsDataAccessTests(FanexDataFixture dataFixture)
        {
            this.dataFixture = dataFixture;
            dataAccess = new CasinoDetailsDataAccess(dataFixture.DynamicRepository, dataFixture.ReportDateService);
        }

        [Fact]
        public void GetCasinoDetails_Always_DynamicRepositoryCallFetch()
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 1, 1, 100);

            // Act
            dataAccess.GetCasinoDetails(param);

            // Assert
            dataFixture.DynamicRepository.Received().Fetch<TicketDto>(Arg.Is<GetCasinoDetailsCriteria>(
                c => c.RefNo == param.RefNo
                && c.CustId == param.CustId
                && c.TransType == param.TransType
                && c.ProductId == param.ProductId
                && c.PageIndex == param.PageIndex
                && c.PageSize == param.PageSize));
        }


        [Fact]
        public void GetCasinoDetails_Always_RetunsPagedData()
        {
            // Arrange
            var compareLogic = new CompareLogic();
            var param = new CasinoDetailsParam(1, 1, 1, 1, 1, 100);
            var criteria = new GetCasinoDetailsCriteria(
                param.RefNo,
                param.CustId,
                param.TransType,
                param.ProductId,
                param.PageIndex,
                param.PageSize,
                DateTime.Today, 
                DateTime.Today);

            dataFixture.DynamicRepository
                .Fetch<TicketDto>(Arg.Any<GetCasinoDetailsCriteria>())
                .Returns(BetListDataTest.Tickets);
            var expectedViewModel = new PagedData<ITicket>(BetListDataTest.Tickets, criteria.PageIndex, criteria.PageSize, criteria.TotalRows);

            // Act
            var actualViewModel = dataAccess.GetCasinoDetails(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, actualViewModel).AreEqual);
        }
    }
}
