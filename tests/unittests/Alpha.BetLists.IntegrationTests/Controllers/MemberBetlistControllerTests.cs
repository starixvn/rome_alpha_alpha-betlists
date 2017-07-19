using System;
using Alpha.BetLists.IntegrationTests.Extensions;
using Alpha.BetLists.IntegrationTests.Fixtures;
using Alpha.BetLists.IntegrationTests.Helpers;
using Alpha.BetLists.StaticData.Configurations;
using Alpha.BetLists.Utils;
using Xunit;

namespace Alpha.BetLists.IntegrationTests.Controllers
{
    public class MemberBetlistControllerTests : BaseBetListsIntegrationTest
    {
        public MemberBetlistControllerTests(AppHostFixture appHostFixture)
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
                SystemTime.Today = new DateTime(2017, 7, 27);
                StaticDataMapper.CriteriaMaps["GetBetListAllByDateCriteria"] = dataFileName;

                var result = session.Get("MemberBetList/Index?CustID=2237314&CustName=member11");

                AssertExtension.Equal(expectedResult.Replace("\r\n", "\n"), result.ResponseText.Replace("\r\n", "\n"));
            });
        }
    }
}
