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
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using static ONEbook.SpreadSheets.Column;

    public class BetListExcelBaseHandler : BetListBaseHandler
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/{0}/{1}.{2}";
        private int currentRowIndex = 1;

        public BetListExcelBaseHandler(
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
            : base(builder, ticketHelper, monitorService, ipService)
        {
            ExcelBuilder = excelBuilder;
            RtfHelper = excelBuilder.RTFHelper;
        }

        protected internal IBetListExcelBuilder ExcelBuilder { get; }

        protected internal RTFHelper RtfHelper { get; }

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

            var mapper = new Mapper<TicketDto>()
                .Map(A, _ => currentRowIndex++)
                .Map(B, ticket => BuildTrans(ticket))
                .Map(C, ticket => BuildChoice(ticket, viewModel))
                .Map(D, ticket => BuildOdds(ticket, viewModel))
                .Map(E, ticket => BuildStake(ticket))
                .Map(F, ticket => BuildWinLoss(ticket))
                .Map(G, ticket => BuildStatus(ticket, viewModel));

            mapper.Map(H, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

            if (viewerLevel.IsMasterLevel())
            {
                mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(I, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
            }

            return mapper;
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
            => ExcelBuilder.CreateStakeBuilder(ticket.BetTypeId).RenderRTF(ticket, RtfHelper);

        public virtual IRichTextString BuildStatus(ITicket ticket, BetListBaseViewModel viewModel)
            => ExcelBuilder
                .CreateStatusBuilder(ticket.BetTypeId)
                .RenderRTF(ticket, TicketHelper, viewModel.BetListData.TicketData, true, RtfHelper);

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
    }
}