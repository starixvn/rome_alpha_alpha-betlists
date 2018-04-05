namespace Alpha.BetLists.Site.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using BetLists.Constants;
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

        public bool IsEnabledJackpot
        {
            get
            {
                var products = GetEnableProductsByGameName(Jackpot);

                return products.Count == 1 && products.Contains(ProductIdConstants.RNGCasino)
                    ? userSession.IsCasinoSync
                    : products.Any();
            }
        }

        public bool HasPermissionOnProduct(int productId) => UserPermission.HasPermissionOnProduct(productId);

        internal virtual IList<int> GetEnableProductsByGameName(string gameName)
            => (IList<int>)HttpContext.Current.Session[HttpContext.Current.Session.SessionID + GameEnableSessionKey + gameName];
    }
}
