namespace Alpha.BetLists.Site.Models
{
    using BetLists.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private readonly IUserSession userSession;
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete
        public User(IUserSession userSession)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            this.userSession = userSession;
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Language => userSession.Language;

        public byte LevelId => userSession.RoleId;

        public int AdminId => userSession.AdminId;

        public bool IsInternal => userSession.IsInternal;

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;
    }
}