namespace Alpha.BetLists.Site.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using NSubstitute;
    using Site.Helpers;

    public static class UrlHelperTestSetUp
    {
        public static void SetUp()
        {
            UrlHelper.MapReportsPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapReportsPath(Arg.Any<string>()).Returns("NewReportsPath");

            UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
        }
    }
}