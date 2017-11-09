namespace Alpha.BetLists.IntegrationTests.Controllers
{
    using System;
    using Alpha.IntegrationTests.Extensions;
    using Alpha.IntegrationTests.Helpers;
    using Fixtures;
    using StaticData.Mappers;
    using StaticData.Models;
    using Utils;
    using Xunit;

    [Trait("Integration", "RunningBetList")]
    public class RunningBetListControllerTests : BaseBetListsIntegrationTest
    {
        public RunningBetListControllerTests(BetListAppHostFixture appHostFixture)
            : base(appHostFixture)
        {
        }

        [Theory]
        [InlineData("EmptyData", "getall-empty-data")]
        [InlineData("RunningBetList", "getall-exist-data")]
        public void Index_GetAllIsTrue_ReturnsCorrectView(string dataFileName, string expectedResultFileName)
        {
            var expectedResult = FileHelper.Load($"ExpectedResults\\RunningBetList\\{expectedResultFileName}.html");

            AppHost.Start(session =>
            {
                // Arrange
                StaticUserSession.SetValues(0, string.Empty, "en", 4, 1, false, false, "E", false, false);
                SystemTime.Today = new DateTime(2017, 7, 27);
                StaticDataMapper.CriteriaMaps["GetBetListRunningByDateCriteria"] = dataFileName;

                // Act
                var result = session.Get("RunningBetList/Index?CustId=11318220&CustName=member011&GetAll=True&ProductIds=1,2,3,4,5,6,8,12,15,21,22,24&SortPropertyName=CustName&IsDescendingSort=false");

                // Assert
                AssertExtension.Equal(expectedResult.StandardizeNewLine(), result.ResponseText.StandardizeNewLine());
            });
        }

        [Theory]
        [InlineData("EmptyData", "support-daterange-empty-data")]
        [InlineData("RunningBetList", "support-daterange-exist-data")]
        public void Index_SupportDateRange_ReturnsCorrectView(string dataFileName, string expectedResultFileName)
        {
            var expectedResult = FileHelper.Load($"ExpectedResults\\RunningBetList\\{expectedResultFileName}.html");
            AppHost.Start(session =>
            {
                // Arrange
                StaticUserSession.SetValues(0, string.Empty, "en", 4, 1, false, false, "E", false, false);
                SystemTime.Today = new DateTime(2017, 7, 27);
                StaticDataMapper.CriteriaMaps["GetBetListRunningByDateCriteria"] = dataFileName;

                // Act
                var result = session.Get("RunningBetList?CustId=11318220&CustName=member011&FromDate=25/06/2017&ToDate=07/25/2017&ProductIds=1,2,3,4,5,6,8,12,15,21,22,24");

                // Assert
                AssertExtension.Equal(expectedResult.StandardizeNewLine(), result.ResponseText.StandardizeNewLine());
            });
        }
    }
}
