using System;
using Xunit;
using Xunit.Sdk;

namespace Alpha.BetLists.IntegrationTests.Extensions
{
    public static class AssertExtension
    {
        public static void Equal(string expected, string actual)
        {
            try
            {
                Assert.Equal(expected, actual);
            }
            catch (XunitException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
