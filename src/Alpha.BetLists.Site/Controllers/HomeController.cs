namespace Alpha.BetLists.Site.Controllers
{
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using System.Threading.Tasks;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private static Random random = new Random(DateTimeOffset.UtcNow.Second);
        private static DynamicRepository repository = new DynamicRepository();

        public ActionResult Index()
        {
            return Content("OK");
        }

        public async Task<ActionResult> BetList()
        {
            var criteria = new JacobCriteria { Index = 1 };
            var model = await repository.FetchAsync<TicketDto>(criteria).ConfigureAwait(false);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Detail()
        {
            var criteria = new JacobCriteria { Index = 2 };
            var model = await repository.FetchAsync<TicketDto>(criteria).ConfigureAwait(false);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }

    public class JacobCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int Index { get; set; }

        public override string GetSettingKey()
        {
            return "JACOB_BetList_Detail_" + Index;
        }

        public override bool IsValid() => true;
    }

}