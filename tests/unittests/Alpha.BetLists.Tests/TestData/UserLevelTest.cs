namespace Alpha.BetLists.Tests.TestData
{
    using System.Diagnostics.CodeAnalysis;
    using BetLists.Models;

    internal class UserLevelTest : UserLevel
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly new UserLevelTest Agent = new UserLevelTest(1, "Super");

        public UserLevelTest()
        {
        }

        private UserLevelTest(byte value, string displayName)
        {
        }
    }
}