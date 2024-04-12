namespace Alpha.BetLists.Site.Features.BookedBet
{
    using Alpha.BetLists.Site.Handlers;
    using Alpha.IPInfo.Services;
    using AlphaPlus.Utilities.IP;
    using AppResources;
    using BetListNPOI.SS.UserModel;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Params;
    using Services;
    using System.Threading.Tasks;
    using ViewModels;

    public class BookedBetExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<BookedBetExcelQuery, ExcelFileViewModel>
    {
        private readonly BookedBetService service;
        private Workbook<TicketDto> workbook;
        private int currentRowIndex = 1;

        public BookedBetExcelHandler(
            BookedBetService service,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            IPService ipService,
            Workbook<TicketDto> workbook,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, null, ipService, ipInfoService)
        {
            this.workbook = workbook;
            this.service = service;
        }

        public async Task<ExcelFileViewModel> Handle(BookedBetExcelQuery message)
        {
            var parameter = new BookedBetParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.ViewerLevelId,
                message.StatusIds,
                message.PageSize,
                message.PageIndex);
            var bookedbets = await service.Get(parameter);
            var viewModel = new BookedBetViewModel(message, bookedbets, Builder, TicketHelper);
            await AssignCountry(bookedbets, message.DisplayVNIP);

            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var filterName = $"{message.FromDate.ToShortDateString()}-->{message.ToDate.ToShortDateString()}";
            var fileContent = workbook
                    .Header(Resources.ResourceManager.GetString)
                    .Title($"{Resources.BookedBetList} ({filterName})")
                    .Body(bookedbets, GetTicketDtoRowMapperExcludeUsername(viewModel))
                    .ExportToBytes();

            return new ExcelFileViewModel($"{Resources.BookedBet}-{message.CustName}-{filterName}", fileContent);
        }

        protected internal override string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        => base.GetExcelTemplatePath("index", fileExtension);

        protected Mapper<TicketDto> GetTicketDtoRowMapperExcludeUsername(BookedBetViewModel viewModel)
            =>new Mapper<TicketDto>()
                .Map(Column.A, _ => currentRowIndex++)
                .Map(Column.B, ticket => ticket.UserName)
                .Map(Column.C, ticket => BuildTrans(ticket))
                .Map(Column.D, ticket => BuildChoice(ticket, viewModel))
                .Map(Column.E, ticket => BuildOdds(ticket, viewModel))
                .Map(Column.F, ticket => BuildStake(ticket))
                .Map(Column.G, ticket => BuildBookedBetStatus(ticket, viewModel));

        private IRichTextString BuildBookedBetStatus(TicketDto ticket, BookedBetViewModel viewModel)
        {
            RtfHelper.RTFRenderer.AddText(ticket.BookedBetStatus);
            var reason = viewModel.GetReason(ticket);
            if (!string.IsNullOrEmpty(reason))
            {
                RtfHelper.RTFRenderer.AddText("\n" + reason);
            }

            if (viewModel.DisplayVNIP)
            {
                RtfHelper.RTFRenderer.AddText("\n" + ticket.IP);
                if (!string.IsNullOrWhiteSpace(ticket.Country))
                {
                    RtfHelper.RTFRenderer.AddText("\n" + ticket.Country);
                }
            }

            var rtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }
    }
}