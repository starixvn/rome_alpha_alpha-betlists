namespace Alpha.BetLists.Site.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using BetLists.Models;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";

        private readonly IUserSession userSession;

        public User(IUserSession userSession)
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

        public string LangKey => userSession.LangKey;

        public bool IsEnabledJackpot => GetEnableProductsForGame(Jackpot).Any();

        public bool HasPermissionOnProduct(int productId) => UserPermission.HasPermissionOnProduct(productId);

        public IList<int> GetEnableProductsForGame(string gameName)
            => (IList<int>)HttpContext.Current.Session[HttpContext.Current.Session.SessionID + GameEnableSessionKey + gameName];
    }
}