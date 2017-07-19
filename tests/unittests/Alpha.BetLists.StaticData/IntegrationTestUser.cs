using Alpha.BetLists.Models;

namespace Alpha.BetLists.StaticData
{
    public class IntegrationTestUser : IUser
    {
        public int Id => 0;

        public string Name => "";

        public string Language => "en";

        public byte LevelId => 4;

        public int AdminId => 1;

        public bool IsInternal => false;

        public bool IsAdminCanViewVNIP => false;

        public string LangKey => "E";
    }
}
