namespace Alpha.BetLists.Site.Handlers
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
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using Parlay;
    using Requests.Parlay;
    using Services;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Parlay;

    using static ONEbook.SpreadSheets.Column;

    public class ParlayExcelHandler : ParlayBaseHandler, IAsyncRequestHandler<ComboParlayDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly ComboParlayDetailWorkbook workbook;

        public ParlayExcelHandler(
            ParlayService parlayService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            ComboParlayDetailWorkbook workbook)
            : base(parlayService, builder, excelBuilder, ticketHelper, monitorService, ipService)
        {
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
                .UseTemplate(GetTemplateInfo(message.ViewerLevel))
                .Sheet(comboParlayDetailViewModel.Title)
                .Title(comboParlayDetailViewModel.Title)
                .Header(Resources.ResourceManager.GetString)
                .ExportToBytes();

            return new ExcelFileViewModel(comboParlayDetailViewModel.Title, fileContent);
        }

        protected internal override TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
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
    }
}