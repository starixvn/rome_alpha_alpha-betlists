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

        public async Task<ActionResult> First()
        {
            var criteria = new JacobCriteria { Index = 1 };
            var model = await repository.FetchAsync<JacobDto>(criteria).ConfigureAwait(false);
            return Json(model);
        }

        public async Task<ActionResult> Second()
        {
            var criteria = new JacobCriteria { Index = 2 };
            var model = await repository.FetchAsync<JacobDto>(criteria).ConfigureAwait(false);
            return Json(model);
        }

        public async Task<ActionResult> Random(int id = 0)
        {
            var loopCount = id <= 0 ? 100 : id;
            var mySet = new HashSet<decimal>();            
            var tasks = new List<Task>();
            for (var i = 0; i < loopCount; i++)
            {
                var task = DoTask(mySet);
                tasks.Add(task);
                if(tasks.Count > 2)
                {
                    var finishedTask = await Task.WhenAny(tasks.ToArray());
                    tasks.Remove(finishedTask);                    
                }
            }

            await Task.WhenAll(tasks.ToArray());

            return Json(mySet);
        }

        private async Task DoTask(HashSet<decimal> mySet)
        {
            var randomIndex = random.Next(1, 3);
            var criteria = new JacobCriteria { Index = randomIndex };
            var model = await repository.FetchAsync<JacobDto>(criteria).ConfigureAwait(false);
            var discount = model.Select(s => s.Discount).ToList();
            discount.ForEach(d => mySet.Add(d));
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