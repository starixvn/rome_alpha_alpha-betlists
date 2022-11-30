namespace Alpha.BetLists.Site.Handlers.Parlay
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AppResources;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using MediatR;
    using BetListNPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using Requests.Parlay;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Parlay;

    using static ONEbook.SpreadSheets.Column;

    public class ParlayExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<ComboParlayDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly ParlayService parlayService;
        private readonly ComboParlayDetailWorkbook workbook;

        public ParlayExcelHandler(
            ParlayService parlayService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            ComboParlayDetailWorkbook workbook)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
            this.workbook = workbook ?? new ComboParlayDetailWorkbookImpl();
            this.parlayService = parlayService;
        }

        public async Task<ExcelFileViewModel> Handle(ComboParlayDetailExcelQuery message)
        {
            var comboParlayDetailViewModel = await GetComboParlayDetail(message, isExportingExcel: true);

            var onebookWorkbook = workbook
                .SetData(comboParlayDetailViewModel)
                .SetRowMapper(
                    DetailsMapper(),
                    DetailsSubTotalMapper(comboParlayDetailViewModel),
                    FooterMapper(comboParlayDetailViewModel, isTotalWinLost: false),
                    FooterMapper(comboParlayDetailViewModel, isTotalWinLost: true))
                .UseTemplate(GetTemplateInfo(message.ViewerLevel));

            RtfHelper = message.RtfHelper ?? InitRTFHelper(workbook.Workbook);

            var fileContent = onebookWorkbook
                .Sheet(comboParlayDetailViewModel.Title)
                .Title(comboParlayDetailViewModel.Title)
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();

            return new ExcelFileViewModel("SystemParlay_Details", fileContent);
        }

        protected internal override TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                  GetExcelTemplatePath("ComboParlayDetail"),
                  noInfomationText: Resources.NoInformationIsAvailable,
                  bodyStartIndex: 3,
                  bodyEndIndex: 5,
                  footerEndIndex: 7);

        private Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsSubTotalMapper(ComboParlayDetailViewModel viewModel)
        {
            var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

            return mapper
                .Map(A, _ => Resources.subTotal + ":")
                .Map(E, details => RenderNumber(viewModel.SubTotalWinLost(details)));
        }

        private Mapper<IEnumerable<ComboParlayDetailWrapper>> FooterMapper(ComboParlayDetailViewModel viewModel, bool isTotalWinLost)
        {
            var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

            mapper
                .Map(A, _ => (isTotalWinLost ? Resources.total : Resources.outstanding) + ":")
                .Map(E, _ => RenderNumber(isTotalWinLost ? viewModel.TotalWinLost : viewModel.OutStanding));

            return mapper;
        }

        private Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsMapper()
        {
            var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

            return mapper.Map(A, details => details.First().WinLostDate)
                .Map(B, details => RenderEvent(details.First()))
                .Map(C, details => RenderNumber(details.First().Odds))
                .Map(D, details => details.First().Stake)
                .Map(E, details => RenderWinLossComm(details.First()))
                .Map(F, details => details.First().Status);
        }

        private IRichTextString RenderEvent(ComboParlayDetailWrapper detail)
        {
            for (var i = 0; i < detail.Events.Count; i++)
            {
                RtfHelper.RTFRenderer.AddText((i > 0 ? "\n" : string.Empty) + detail.Events[i]);
            }

            var eventRtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return eventRtfText;
        }

        private IRichTextString RenderWinLossComm(ComboParlayDetailWrapper detail)
        {
            var rtfNumber = new RTFNumber(RtfHelper.RTFRenderer, RtfHelper.PosFont, RtfHelper.NegFont);

            rtfNumber.Render(detail.WinLost + "\n");
            rtfNumber.Render(detail.Comm);

            var rtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }

        private IRichTextString RenderNumber(string numberValue)
        {
            var rtfNumber = new RTFNumber(RtfHelper.RTFRenderer, RtfHelper.PosFont, RtfHelper.NegFont);

            rtfNumber.Render(numberValue);

            var rtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }

        private async Task<ComboParlayDetailViewModel> GetComboParlayDetail(ComboParlayDetailQuery message, bool isExportingExcel = false)
        {
            var comboParlayDetailData = await parlayService.GetComboParlayDetail(
                message.TransId,
                message.RefNo,
                message.BetId,
                message.CustId,
                message.Language);

            return new ComboParlayDetailViewModel(message, comboParlayDetailData, TicketHelper, isExportingExcel);
        }
    }
}