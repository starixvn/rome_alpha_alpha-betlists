namespace Alpha.BetLists.Site.Tests.Handler.Parlay
{
    using System.Collections.Generic;
    using BetLists.Tests.TestData;
    using Fixtures;
    using Handlers.Parlay;
    using NSubstitute;
    using ONEbook.SpreadSheets;
    using Requests.Parlay;
    using Site.ViewModels.Parlay;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class ComboParlayDetailWorkbookTests
    {
        private readonly ComboParlayDetailWorkbookImpl workbook;
        private readonly ComboParlayDetailExcelQuery query;
        private readonly BetListFixture betListFixture;
        private readonly Sheet sheet;

        public ComboParlayDetailWorkbookTests(BetListFixture betListFixture)
        {
            workbook = new ComboParlayDetailWorkbookImpl();
            this.betListFixture = betListFixture;
            query = new ComboParlayDetailExcelQuery(this.betListFixture.User);
            query.RtfHelper = this.betListFixture.RtfHelper;
            sheet = Substitute.For<Sheet>();
        }

        [Fact]
        public void SetData_Always_SetComboParlayDetails()
        {
            // Arrange
            var comboParlayDetails = new ComboParlayDetailViewModel(
                query,
                BetListDataTest.ComboParlayDetailData,
                betListFixture.TicketHelper);

            // Act
            workbook.SetData(comboParlayDetails);

            // Assert
            Assert.Equal(workbook.ComboParlayDetails, comboParlayDetails);
        }

        [Fact]
        public void SetRowMapper_Always_SetMappers()
        {
            // Arrange
            var detailsMapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();
            var detailsSubTotalMapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();
            var footerOutstandingMapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();
            var footerTotalWinLostMapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

            // Act
            workbook.SetRowMapper(detailsMapper, detailsSubTotalMapper, footerOutstandingMapper, footerTotalWinLostMapper);

            // Assert
            Assert.True(workbook.DetailsMapper == detailsMapper
                && workbook.DetailsSubTotalMapper == detailsSubTotalMapper
                && workbook.FooterOutstandingMapper == footerOutstandingMapper
                && workbook.FooterTotalWinLostMapper == footerTotalWinLostMapper);
        }
    }
}