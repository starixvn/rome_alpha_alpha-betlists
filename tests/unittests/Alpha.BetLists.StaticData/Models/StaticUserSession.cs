namespace Alpha.BetLists.StaticData.Models
{
    using System;
    using BetLists.Models;

    public class StaticUserSession : IUser
    {
        [ThreadStatic]
        private static int id;

        [ThreadStatic]
        private static string name;

        [ThreadStatic]
        private static string language;

        [ThreadStatic]
        private static byte levelId;

        [ThreadStatic]
        private static int adminId;

        [ThreadStatic]
        private static bool isInternal;

        [ThreadStatic]
        private static bool isAdminCanViewVNIP;

        [ThreadStatic]
        private static string langKey;

        [ThreadStatic]
        private static bool isShowRacing;

        public static void SetValues(
            int id,
            string name,
            string language,
            byte levelId,
            int adminId,
            bool isInternal,
            bool isAdminCanViewVNIP,
            string langKey,
            bool isShowRacing)
        {
            StaticUserSession.id = id;
            StaticUserSession.name = name;
            StaticUserSession.language = language;
            StaticUserSession.levelId = levelId;
            StaticUserSession.adminId = adminId;
            StaticUserSession.isInternal = isInternal;
            StaticUserSession.isAdminCanViewVNIP = isAdminCanViewVNIP;
            StaticUserSession.langKey = langKey;
            StaticUserSession.isShowRacing = isShowRacing;
        }

        public int Id => id;

        public string Name => name;

        public string Language => language;

        public byte LevelId => levelId;

        public int AdminId => adminId;

        public bool IsInternal => isInternal;

        public bool IsAdminCanViewVNIP => isAdminCanViewVNIP;

        public string LangKey => langKey;

        public bool IsShowRacing => isShowRacing;
    }
}
