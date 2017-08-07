using System;
using Alpha.BetLists.IntegrationTests.Extensions;
using Alpha.BetLists.IntegrationTests.Fixtures;
using Alpha.BetLists.IntegrationTests.Helpers;
using Alpha.BetLists.StaticData.Configurations;
using Alpha.BetLists.StaticData.Models;
using Alpha.BetLists.Utils;
using Xunit;

namespace Alpha.BetLists.IntegrationTests.Controllers
{
    public class RunningBetListControllerTests : BaseBetListsIntegrationTest
    {
        public RunningBetListControllerTests(BetListAppHostFixture appHostFixture)
            : base(appHostFixture)
        {
        }

        [Theory]
        [InlineData("EmptyData", "empty-data")]
        [InlineData("RunningBetList", "exist-data")]
        public void Index_Always_ReturnsCorrectView(string dataFileName, string expectedResultFileName)
        {
            var expectedResult = FileHelper.Load($"ExpectedResults\\RunningBetList\\{expectedResultFileName}.html");

            AppHost.Start(session =>
            {
                // Arrange
                StaticUserSession.SetValues(0, string.Empty, "en", 4, 1, false, false, "E");
                SystemTime.Today = new DateTime(2017, 7, 27);
                StaticDataMapper.CriteriaMaps["GetBetListRunningByDateCriteria"] = dataFileName;

                // Act
                var result = session.Get("RunningBetList/Index?CustId=11318220&CustName=member011&FromDate=07/19/2017&ToDate=07/26/2017&ProductIds=1,2,3,4,5,6,8,12,15,21,22,24&SortPropertyName=CustName&IsDescendingSort=false");

                // Assert
                AssertExtension.Equal(expectedResult.StandardizeNewLine(), result.ResponseText.StandardizeNewLine());
            });
        }
    }
}
