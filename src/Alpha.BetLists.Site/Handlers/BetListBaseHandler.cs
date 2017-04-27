namespace Alpha.BetLists.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using AppResources;
    using BetLists.Models;
    using Builder.ExcelBuilders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using SmartPunter.Services;
    using Utilities.IP;
    using ViewModels;
    using ViewModels.Blocks;
    using static ONEbook.SpreadSheets.Column;

    public class BetListBaseHandler
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/{0}.{1}";
        private Func<string, string> mapPath;

        public BetListBaseHandler(
            IBetListHTMLBuilder builder,
            IBetListExcelBuilder excelBuilder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
        {
            Builder = builder;
            ExcelBuilder = excelBuilder;
            TicketHelper = ticketHelper;
            RtfHelper = excelBuilder.RTFHelper;
            MonitorService = monitorService;
            IpService = ipService;
            TicketHelper.IsVnIp = IpService.IsVNIP;
        }

        protected internal IBetListHTMLBuilder Builder { get; }

        protected internal IBetListExcelBuilder ExcelBuilder { get; }

        protected internal ITicketHelper TicketHelper { get; }

        protected internal RTFHelper RtfHelper { get; }

        protected internal ISmartPunterBetListMonitor MonitorService { get; }

        protected internal IPService IpService { get; }

        protected internal GetCachePropertyById GetOddsTypeName { get; set; } = CacheRef.GetOddsTypeName;

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HostingEnvironment.MapPath; }
            set { mapPath = value; }
        }

        protected internal void WriteLog(int custId, IEnumerable<ITicket> tickets)
             => MonitorService.WriteLog(custId, tickets.ToList());

        #region Excel

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var relativePath = string.Format(ExcelTemplateDir, templateName, fileExtension);

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }

        protected internal virtual TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                   GetExcelTemplatePath(viewerLevel.DisplayName),
                   noInfomationText: Resources.NoInformationIsAvailable,
                   bodyStartIndex: 3,
                   bodyEndIndex: 4,
                   footerEndIndex: 5);

        protected internal virtual Mapper<ITicket> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
        {
            var ticketData = viewModel.BetListData.TicketData;
            var canViewScoreMap = viewModel.CanViewScoreMap;

            var mapper = new Mapper<ITicket>()
                .Map(B, ticket => ExcelBuilder.CreateTransBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper))
                .Map(C, ticket => ExcelBuilder.CreateChoiceBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper, ticketData, canViewScoreMap, RtfHelper).FirstOrDefault())
                .Map(D, ticket => ExcelBuilder.CreateOddsBuilder(ticket.BetTypeId).RenderRTF(ticket, ticketData, GetOddsTypeName, RtfHelper))
                .Map(E, ticket => ExcelBuilder.CreateStakeBuilder(ticket.BetTypeId).RenderRTF(ticket, RtfHelper))
                .Map(F, ticket => BuildWinLoss(ticket))
                .Map(G, ticket => ExcelBuilder.CreateStatusBuilder(ticket.BetTypeId).RenderRTF(ticket, TicketHelper, ticketData, true, RtfHelper));

            if (viewerLevel.IsAgentLevel())
            {
                mapper.Map(H, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (viewerLevel.IsMasterLevel())
            {
                mapper
                    .Map(H, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(I, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            if (viewerLevel.IsSuperLevel())
            {
                mapper
                    .Map(H, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Super))
                    .Map(I, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Master))
                    .Map(J, ticket => BuildPTCommmission(ticket, ticketData, UserLevel.Agent));
            }

            return mapper;
        }

        protected internal virtual Mapper<ITicket> GetFooterRowMapper(Footer footer)
        {
            var mapper = new Mapper<ITicket>()
                .Map(A, _ => BetListExcelBuilderExt.BuildFooterHeader(RtfHelper))
                .Map(F, _ => BuildFooter(footer));

            return mapper;
        }

        protected virtual IRichTextString BuildWinLoss(ITicket ticket)
        {
            var winLossBlockViewModel = new WinLostBlockViewModel(ticket);

            return BetListExcelBuilderExt.BuildWinLoss(
                winLossBlockViewModel.WinLost,
                winLossBlockViewModel.CommVisibility ? winLossBlockViewModel.Comm : string.Empty,
                RtfHelper);
        }

        protected virtual IRichTextString BuildPTCommmission(ITicket ticket, List<ITicketData> ticketData, UserLevel userLevel)
            => BetListExcelBuilderExt.BuildPTCommmission(ticket, ticketData, userLevel, RtfHelper);

        protected virtual IRichTextString BuildFooter(Footer footerData)
            => BetListExcelBuilderExt.BuildFooter(
                footerData.SubTotalWinLoss,
                footerData.SubTotalCommission,
                footerData.Total,
                RtfHelper);

        #endregion Excel
    }
}