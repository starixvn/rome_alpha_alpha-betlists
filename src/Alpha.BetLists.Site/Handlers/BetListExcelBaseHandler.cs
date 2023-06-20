namespace Alpha.BetLists.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AppResources;
    using BetLists.Models;
    using Builders.ExcelBuilders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using BetListNPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using static ONEbook.SpreadSheets.Column;
    using Alpha.IPInfo.Services;

    /// <summary>
    /// Bet list excel base builder for default method that help to export Excel.
    /// Please kindly note that excel file needs to be opened with Microsoft Office Excel.
    /// If you got the issue about excel file's cell show text in only one line.
    /// Please add empty line to cell in Template Excel File
    /// </summary>
    public class BetListExcelBaseHandler : BetListBaseHandler
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/{0}/{1}.{2}";
        private int currentRowIndex = 1;

        public BetListExcelBaseHandler(
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
            : base(builder, ticketHelper, monitorService, ipService, ipInfoService)
        {
            ExcelBuilder = excelBuilder;
            RtfHelper = excelBuilder.RTFHelper;
        }

        protected internal IBetListExcelBuilder ExcelBuilder { get; }

        protected internal RTFHelper RtfHelper { get; set; }

        /// <summary>
        /// Inititalize RTFHelper for create Font of Workbook for rendering correct format.
        /// Need to call this method after calling UseTemplate method of ONEBook.SpreadSheet
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        protected internal static RTFHelper InitRTFHelper(IWorkbook workbook)
        {
            var rtfHelper = new RTFHelper
            {
                NegFont = workbook.CreateFont(),
                NormalFont = workbook.CreateFont(),
                PosFont = workbook.CreateFont(),
                NegFontCrossed = workbook.CreateFont(),
                NormalFontCrossed = workbook.CreateFont(),
                PosFontCrossed = workbook.CreateFont(),
                RTFRenderer = new RtfTextRender()
            };

            InitRTFHelperFonts(rtfHelper);

            return rtfHelper;
        }

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var relativePath = string.Format(ExcelTemplateDir, GetExcelTemplateFolder(), templateName, fileExtension);

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }

        protected internal string GetExcelTemplateFolder()
        {
            var reportNamespace = GetType().Namespace;

            var lastIndexOfDot = reportNamespace.LastIndexOf(".", StringComparison.Ordinal) + 1;

            if (lastIndexOfDot <= 0)
            {
                throw new ArgumentException("Namespace does not contain any dot character, wrong report convention.");
            }

            var templateFolderName = reportNamespace.Substring(lastIndexOfDot);

            return templateFolderName;
        }

        protected internal virtual TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                   ExcelTemplatePath(viewerLevel),
                   noInfomationText: Resources.NoInformationIsAvailable,
                   bodyStartIndex: 3,
                   bodyEndIndex: 4,
                   footerEndIndex: 5);

        protected internal virtual string ExcelTemplatePath(UserLevel viewerLevel)
            => GetExcelTemplatePath(viewerLevel.DisplayName);

        protected internal virtual Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;

            var mapper = GetTicketDtoRowMapperExcludeUsername(viewModel);

            mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            AddPtCommissionForSuper(mapper, viewerLevel, ticketData);

            return mapper;
        }

        protected Mapper<TicketDto> GetTicketDtoRowMapperIncludeUsername(BetListBaseViewModel viewModel, bool showWinLoss)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => ticket.UserName)
                .Map(C, ticket => BuildTrans(ticket))
                .Map(D, ticket => BuildChoice(ticket, viewModel))
                .Map(E, ticket => BuildOdds(ticket, viewModel))
                .Map(F, ticket => BuildStake(ticket));

            if (showWinLoss)
            {
                mapper
                    .Map(G, ticket => BuildWinLoss(ticket))
                    .Map(H, ticket => BuildStatus(ticket, viewModel));
                return mapper;
            }


            mapper.Map(G, ticket => BuildStatus(ticket, viewModel));
            return mapper;
        }

        protected Mapper<TicketDto> GetTicketDtoRowMapperExcludeUsername(BetListBaseViewModel viewModel)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => BuildTrans(ticket))
                .Map(C, ticket => BuildChoice(ticket, viewModel))
                .Map(D, ticket => BuildOdds(ticket, viewModel))
                .Map(E, ticket => BuildStake(ticket))
                .Map(F, ticket => BuildWinLoss(ticket))
                .Map(G, ticket => BuildStatus(ticket, viewModel));

            return mapper;
        }

        protected void AddPtCommissionForSuper(Mapper<TicketDto> mapper, UserLevel viewerLevel, List<ITicketData> ticketData)
        {
            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }
        }

        protected internal virtual Mapper<TicketDto> GetFooterRowMapper(Footer footer)
        {
            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => BetListExcelBuilderExt.BuildFooterHeader(RtfHelper))
                .Map(F, _ => BuildFooter(footer));

            return mapper;
        }

        protected virtual IRichTextString BuildTrans(ITicket ticket)
            => ExcelBuilder.CreateTransBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper);

        protected virtual IRichTextString BuildChoice(ITicket ticket, BetListBaseViewModel viewModel)
            => ExcelBuilder
                .CreateChoiceBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, TicketHelper, viewModel.BetListData.TicketData, viewModel.CanViewScoreMap(ticket), RtfHelper)
                .FirstOrDefault();

        protected virtual IRichTextString BuildOdds(ITicket ticket, BetListBaseViewModel viewModel)
            => ExcelBuilder
                .CreateOddsBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, viewModel.BetListData.TicketData, GetOddsTypeName, RtfHelper);

        public virtual IRichTextString BuildStake(ITicket ticket)
            => ExcelBuilder.CreateStakeBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper, RtfHelper);

        public virtual IRichTextString BuildStatus(TicketDto ticket, BetListBaseViewModel viewModel)
        {
            RtfHelper.RTFRenderer.AddText(ExcelBuilder
                .CreateStatusBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, TicketHelper, viewModel.BetListData.TicketData, viewModel.DisplayVNIP, RtfHelper)
                .ToString());

            if (!string.IsNullOrWhiteSpace(ticket.Country))
            {
                RtfHelper.RTFRenderer.AddText("\n" + ticket.Country);
            }

            var rtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }

        protected virtual IRichTextString BuildWinLoss(ITicket ticket)
        {
            var winLossBlockViewModel = new WinLostBlockViewModel(ticket);

            return BetListExcelBuilderExt.BuildWinLoss(
                winLossBlockViewModel.WinLost,
                winLossBlockViewModel.CommVisibility ? winLossBlockViewModel.Comm : string.Empty,
                RtfHelper);
        }

        protected virtual IRichTextString BuildPtCommmission(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
            => BetListExcelBuilderExt.BuildPtCommmission(ticket, ticketData, userLevel, RtfHelper);

        protected virtual IRichTextString BuildFooter(Footer footerData)
            => BetListExcelBuilderExt.BuildFooter(
                footerData.SubTotalWinLoss,
                footerData.SubTotalCommission,
                footerData.Total,
                RtfHelper);

        private static void InitRTFHelperFonts(RTFHelper rtfHelper)
        {
            rtfHelper.PosFont.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.PosFontCrossed.Boldweight = (short)FontBoldWeight.Bold;

            rtfHelper.NegFont.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.NegFontCrossed.Boldweight = (short)FontBoldWeight.Bold;

            rtfHelper.NormalFont.Boldweight = (short)FontBoldWeight.Normal;
            rtfHelper.NormalFontCrossed.Boldweight = (short)FontBoldWeight.Normal;

            rtfHelper.NegFont.FontName = "Tahoma";
            rtfHelper.NegFontCrossed.FontName = "Tahoma";
            rtfHelper.NormalFont.FontName = "Tahoma";
            rtfHelper.NormalFontCrossed.FontName = "Tahoma";
            rtfHelper.PosFont.FontName = "Tahoma";
            rtfHelper.PosFontCrossed.FontName = "Tahoma";

            rtfHelper.NegFont.FontHeightInPoints = 9;
            rtfHelper.NegFontCrossed.FontHeightInPoints = 9;
            rtfHelper.NormalFont.FontHeightInPoints = 9;
            rtfHelper.NormalFontCrossed.FontHeightInPoints = 9;
            rtfHelper.PosFont.FontHeightInPoints = 9;
            rtfHelper.PosFontCrossed.FontHeightInPoints = 9;

            rtfHelper.NormalFontCrossed.IsStrikeout = true;
            rtfHelper.PosFontCrossed.IsStrikeout = true;
            rtfHelper.NegFontCrossed.IsStrikeout = true;

            rtfHelper.NegFont.Color = (short)FontColor.Red;
            rtfHelper.NegFontCrossed.Color = (short)FontColor.Red;
        }
    }
}
