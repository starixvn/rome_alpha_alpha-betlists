namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BetLists.Models;
    using MediatR;
    using Requests.BullfightingByMatch;

    public class BullfightingByMatchController : BetListBaseController
    {
        private readonly IUser viewer;

        public BullfightingByMatchController(IMediator mediator, IUser viewer) 
            : base(mediator)
        {
            this.viewer = viewer;
        }

        public async Task<ActionResult> Index(BullfightingByMatchQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(BullfightingByMatchExcelQuery message)
            => await ExcelView(message);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if(!viewer.IsShowBullfighting)
            {
                UnAuthentication(filterContext);
            }
        }
    }
}
