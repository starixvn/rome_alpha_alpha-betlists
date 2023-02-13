namespace Alpha.BetLists.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.NetThrottling.Mvc;
    using Alpha.Web.Filters;

    [AllowAnonymous, AllowProtectedMode, SessionState(SessionStateBehavior.ReadOnly)]
    public class ErrorController : Controller
    {
        [DisableNetThrottling]
        public ActionResult Blocked() => View();
    }
}