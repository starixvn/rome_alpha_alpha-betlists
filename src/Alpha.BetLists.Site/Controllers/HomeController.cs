namespace Alpha.BetLists.Site.Controllers
{
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using System.Linq;
    using System;
    using Fanex.Data;
    using System.Collections.Generic;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index(int id = 0)
        {
            id = id <= 0 ? 100 : id;
            var repository = new DynamicRepository();
            var mySet = new HashSet<decimal>();

            for (var i = 0; i < id; i++)
            {
                var criteria = new GetCasinoDetailsCriteria(
                   240528025538527232,
                   65596555,
                   1,
                   21,
                   1,
                   500,
                   new DateTime(2024, 4, 5),
                   new DateTime(2024, 3, 20));

                var tickets = repository.Fetch<TicketDto>(criteria).ToList();
                var odds = tickets.Select(s => s.MasterDiscount).ToList();
                odds.ForEach(o => mySet.Add(o));
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(mySet);
            ViewBag.MyJson = json;
            return View();
        }
    }

    public class GetCasinoDetailsCriteria : CriteriaBase
    {
        public long RefNo { get; }

        public int CustId { get; }

        public byte TransType { get; }

        [SpParam(Ignored = true)]
        public int ProductId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRows { get; set; }

        [SpParam(SqlParamName = "movebetsday")]
        public DateTime MoveBetsDate { get; }

        [SpParam(SqlParamName = "BalanceUpDay")]
        public DateTime BalanceUpDate { get; }

        public GetCasinoDetailsCriteria(long refNo, int custId, byte transType, int productId, int pageIndex, int pageSize, DateTime moveBetsDate, DateTime balanceUpDate)
        {
            RefNo = refNo;
            CustId = custId;
            TransType = transType;
            ProductId = productId;
            PageIndex = pageIndex;
            PageSize = pageSize;
            MoveBetsDate = moveBetsDate;
            BalanceUpDate = balanceUpDate;
        }

        public override string GetSettingKey()
        {
            var random = new Random(DateTimeOffset.UtcNow.Second);
            return "Acc_BetList_Detail_" + random.Next(0, 100);
        }

        public override bool IsValid()
        {
            return RefNo > 0 && CustId > 0 && ProductId > 0;
        }
    }

}