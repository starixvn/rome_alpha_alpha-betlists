namespace Alpha.BetLists.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BetLists.Models;
    using MediatR;
    using Requests.RacingByMatch;

    public class RacingByMatchController : BetListBaseController
    {
        private readonly IUser viewer;

        public RacingByMatchController(IMediator mediator, IUser viewer) 
            : base(mediator)
        {
            this.viewer = viewer;
        }

        public async Task<ActionResult> Index(RacingByMatchQuery message)
            => await View(message);

        public async Task<FileContentResult> Excel(RacingByMatchExcelQuery message)
            => await ExcelView(message);

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if(!viewer.IsShowRacing)
            {
                UnAuthentication(filterContext);
            }
        }
    }
}
