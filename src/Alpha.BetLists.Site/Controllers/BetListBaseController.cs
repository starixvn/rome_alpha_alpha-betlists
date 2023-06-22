namespace Alpha.BetLists.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web.Models;
    using MediatR;
    using ViewModels;
    using Web;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class BetListBaseController : BaseAsyncController
    {
        protected const string InvalidBetListView = "~/Views/Shared/InvalidBetList.cshtml";
        protected readonly IMediator mediator;

        protected BetListBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message)
            => View(await mediator.Send(message));

        protected async Task<ActionResult> View<TResponse>(string viewName, IRequest<TResponse> message)
            => View(viewName, await mediator.Send(message));

        protected async Task<ActionResult> PartialView<TResponse>(IRequest<TResponse> message)
            => PartialView(await mediator.Send(message));

        protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
            => GetExcelContent(await mediator.Send(message));

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message, string viewName)
            => View(viewName, await mediator.Send(message));

        protected async Task<ActionResult> Json<TResponse>(
            IRequest<TResponse> message,
            JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            => Json(await mediator.Send(message), jsonRequestBehavior);

        protected ActionResult JsonForbidden(JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            => Json(new Result((int)HttpStatusCode.Forbidden, AppResources.Resources.NotAuthorized), jsonRequestBehavior);
    }
}