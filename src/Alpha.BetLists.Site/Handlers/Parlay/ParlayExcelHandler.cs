namespace Alpha.BetLists.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AppResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using MediatR;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using Parlay;
    using Requests.Parlay;
    using Services;
    using ViewModels;
    using ViewModels.Parlay;
    using static ONEbook.SpreadSheets.Column;

    public class ParlayExcelHandler : ParlayBaseHandler, IAsyncRequestHandler<ComboParlayDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly ComboParlayDetailWorkbook workbook;
        private RTFHelper rtfHelper;

        public ParlayExcelHandler(
            ParlayService parlayService,
            ITicketHelper ticketHelper,
            IBetListExcelBuilder excelBuilder,
            ComboParlayDetailWorkbook workbook) : base(parlayService, ticketHelper)
        {
            this.rtfHelper = excelBuilder.RTFHelper;
            this.workbook = workbook ?? new ComboParlayDetailWorkbookImpl();
        }

        public async Task<ExcelFileViewModel> Handle(ComboParlayDetailExcelQuery message)
        {
            var comboParlayDetailViewModel = await GetComboParlayDetail(message);

            var fileContent = workbook
                .SetData(comboParlayDetailViewModel)
                .SetRowMapper(
                    DetailsMapper(),
                    DetailsSubTotalMapper(comboParlayDetailViewModel),
                    FooterMapper(comboParlayDetailViewModel, isTotalWinLost: false),
                    FooterMapper(comboParlayDetailViewModel, isTotalWinLost: true))
                .UseTemplate(GetTemplateInfo())
                .Sheet(comboParlayDetailViewModel.Title)
                .Title(comboParlayDetailViewModel.Title)
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();

            return new ExcelFileViewModel(comboParlayDetailViewModel.Title, fileContent);
        }

        private TemplateInfo GetTemplateInfo()
           => new TemplateInfo(
                  GetExcelTemplatePath("ComboParlayDetail"),
                  noInfomationText: Resources.NoInformationIsAvailable,
                  bodyStartIndex: 3,
                  bodyEndIndex: 5,
                  footerEndIndex: 7);

        private Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsMapper()
        {
            var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

            return mapper.Map(A, details => details.First().WinLostDate)
                .Map(B, details => RenderEvent(details.First()))
                .Map(C, details => details.First().Odds)
                .Map(D, details => details.First().Stake)
                .Map(E, details => RenderWinLossComm(details.First()))
                .Map(F, details => details.First().Status);
        }

        private Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsSubTotalMapper(ComboParlayDetailViewModel viewModel)
        {
            var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

            return mapper
                .Map(A, _ => Resources.subTotal + ":")
                .Map(E, details => viewModel.SubTotalWinLost(details));
        }

        private Mapper<IEnumerable<ComboParlayDetailWrapper>> FooterMapper(ComboParlayDetailViewModel viewModel, bool isTotalWinLost)
        {
            var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

            mapper
                .Map(A, _ => (isTotalWinLost ? Resources.total : Resources.outstanding) + ":")
                .Map(E, _ => isTotalWinLost ? viewModel.TotalWinLost : viewModel.OutStanding);

            return mapper;
        }

        private IRichTextString RenderEvent(ComboParlayDetailWrapper detail)
        {
            for (var i = 0; i < detail.Events.Count(); i++)
            {
                rtfHelper.RTFRenderer.AddText((i > 0 ? "\n" : string.Empty) + detail.Events[i]);
            }

            var eventRtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return eventRtfText;
        }

        private IRichTextString RenderWinLossComm(ComboParlayDetailWrapper detail)
        {
            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            rtfNumber.Render(detail.WinLost + "\n");
            rtfNumber.Render(detail.Comm);

            var rtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfText;
        }
    }
}