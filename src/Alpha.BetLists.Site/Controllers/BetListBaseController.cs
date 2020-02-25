namespace Alpha.BetLists.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.BetLists.Site.Requests;
    using MediatR;
    using ViewModels;
    using Web;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class BetListBaseController : BaseAsyncController
    {
        private const string InvalidReportViewPath = "~/Views/Shared/InvalidBetList.cshtml";
        private const string InvalidReportExcelFileName = "InvalidReport";

        protected readonly IMediator mediator;

        protected BetListBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message)
        {
            var baseQuery = message as BaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return View(InvalidReportViewPath);
            }

            return View(await mediator.Send(message));
        }

        protected async Task<ActionResult> PartialView<TResponse>(IRequest<TResponse> message)
        {
            var baseQuery = message as BaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return View(InvalidReportViewPath);
            }

            return PartialView(await mediator.Send(message));
        }

        protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var baseQuery = message as BaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidReportExcelFileName, new byte[] { }));
            }

            return GetExcelContent(await mediator.Send(message));
        }
    }
}