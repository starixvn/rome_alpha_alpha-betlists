namespace Alpha.BetLists.Site.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.NetThrottling.Mvc;
    using Alpha.Web.Filters;

    [AllowAnonymous]
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            var repository = new DynamicRepository();
            var criteria = new GetCasinoDetailsCriteria(
               param.RefNo,
               param.CustId,
               param.TransType,
               param.ProductId,
               param.PageIndex,
               param.PageSize,
               reportDateService.GetMoveBetsDay(),
               reportDateService.GetBalanceUpDay());

            var tickets = repository.Fetch<TicketDto>(criteria);
            return View();
        } 
    }
}