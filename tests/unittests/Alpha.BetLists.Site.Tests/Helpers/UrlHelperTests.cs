namespace Alpha.BetLists.Site.Tests.Helpers
{
    using NSubstitute;
    using Site.Helpers;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "Site.Helpers")]
    public class UrlHelperTests
    {
        [Fact]
        public void AddQueryStringParams_EmptyUrl_ReturnQuestionMarkAddedQueryString()
        {
            // Arrange
            var url = string.Empty;
            var routeValue = new Dictionary<string, object>
            {
                { "paramName", "paramValue" },
                { "nullValue", null },
                { "emptyValue", string.Empty }
            };

            // Act
            var actualUrl = UrlHelper.AddQueryStringParams(url, routeValue);

            // Assert
            Assert.Equal("?paramName=paramValue&nullValue=&emptyValue=", actualUrl);
        }

        [Fact]
        public void AddQueryStringParams_EmptyRouteValues_ReturnInputUrl()
        {
            // Arrange
            var url = "url";
            var routeValue = new Dictionary<string, object>();

            // Act
            var actualUrl = UrlHelper.AddQueryStringParams(url, routeValue);

            // Assert
            Assert.Equal("url", actualUrl);
        }

        [Fact]
        public void GetDailyStatementLink_Always_ReturnsCorrectLink()
        {
            // Arrange

            UrlHelperTestSetUp.SetUp();

            // Act
            UrlHelper.GetDailyStatementLink(1, "CustName", "1,2,3");

            // Assert
            UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewReportsPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValues =>
                    (int)routeValues["CustId"] == 1
                    && (string)routeValues["CustName"] == "CustName"
                    && (int)routeValues["CustLevelId"] == 1
                    && (string)routeValues["ProductIds"] == "1,2,3"));
        }
    }
}