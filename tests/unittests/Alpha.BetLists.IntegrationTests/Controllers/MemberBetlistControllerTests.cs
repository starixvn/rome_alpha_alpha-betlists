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
    public class MemberBetlistControllerTests : BaseBetListsIntegrationTest
    {
        public MemberBetlistControllerTests(BetListAppHostFixture appHostFixture)
            : base(appHostFixture)
        {
        }

        [Theory]
        [InlineData("EmptyData", "empty-data")]
        [InlineData("MemberBetList", "exist-data")]
        public void Index_Always_ReturnsCorrectView(string dataFileName, string expectedResultFileName)
        {
            var expectedResult = FileHelper.Load($"ExpectedResults\\MemberBetList\\{expectedResultFileName}.html");

            AppHost.Start(session =>
            {
                // Arrange
                StaticUserSession.SetValues(0, string.Empty, "en", 4, 1, false, false, "E");
                SystemTime.Today = new DateTime(2017, 7, 27);
                StaticDataMapper.CriteriaMaps["GetBetListAllByDateCriteria"] = dataFileName;

                // Act
                var result = session.Get("MemberBetList/Index?CustID=2237314&CustName=member11");

                // Assert
                AssertExtension.Equal(expectedResult.StandardizeNewLine(), result.ResponseText.StandardizeNewLine());
            });
        }
    }
}
