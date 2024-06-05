namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using Alpha.IPInfo.Services;
    using AlphaPlus.SmartPunter.Services;
    using AlphaPlus.Utilities.IP;
    using AppResources;
    using BetLists.Models;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.BetListNPOI;
    using Params;
    using Requests.WinLoss;
    using Services;
    using System;
    using System.Threading.Tasks;
    using ViewModels;
    using ViewModels.WinLoss;

    public class WinLossExcelHandler : BetListExcelBaseHandler, IAsyncRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
    {
        private readonly WinLossService winLossService;
        private readonly ProductService productService;
        private readonly ProductDetailService productDetailService;
        private Workbook<TicketDto> workbook;

        public WinLossExcelHandler(
            WinLossService winLossService,
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            Workbook<TicketDto> workbook,
            ProductService productService,
            ProductDetailService productDetailService,
            IIPInfoService ipInfoService)
            : base(builder, excelBuilder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            this.workbook = workbook;
            this.winLossService = winLossService;
            this.productService = productService;
            this.productDetailService = productDetailService;
        }

        public async Task<ExcelFileViewModel> Handle(WinLossExcelQuery message)
        {
            var productByHLParam = new GetProductsByGroupHeadlineParam(message.ViewerId, message.MinDate);
            var validUserSelectedProductIds = message.IsGroupHeadline
                ? productService.FilterProductIdsByGroupHeadline(message.ProductIds.ToListInt(), productByHLParam)
                : productService.FilterProductIds(message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt());
            var validResourceIds = await productDetailService.FilterResourceIdsByCustomer(message.Site, message.CurrencyId, message.ResourceIds);
            var productDetailsXML = await productDetailService.BuildProductDetailsXMLAsync(validResourceIds, message.Is789Y);

            var winLossParam = WinLossHandler.CreateWinLossParam(message, validUserSelectedProductIds, productDetailsXML, message.MaxTotalRowExportExcel, message.MaxTotalRowExportExcelConfig);
            winLossParam.IsExcel = true;
            var betlistData = await winLossService.GetBetList(winLossParam);
            await AssignCountry(betlistData.Tickets, message.DisplayVNIP);

            var winLossViewModel = new WinLossViewModel(message, betlistData, null, TicketHelper, false, betlistOutputParams: betlistData.BetlistOutputParams);
            workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
            RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

            var fileContent = workbook
                    .Header(Resources.ResourceManager.GetString)
                    .Title(string.IsNullOrWhiteSpace(winLossViewModel.BetlistOutputParams.ErrMsg) ?
                        winLossViewModel.Title : $"{winLossViewModel.BetlistOutputParams.ErrMsg}{Environment.NewLine}{winLossViewModel.Title}")
                    .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, winLossViewModel))
                    .Footer(new TicketDto(), GetFooterRowMapper(winLossViewModel.Footer))
                    .ExportToBytes();

            return new ExcelFileViewModel(winLossViewModel.Title, fileContent);
        }
    }
}