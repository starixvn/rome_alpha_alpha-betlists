namespace Alpha.BetLists.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using SmartPunter.Services;
    using Utilities.IP;

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

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HostingEnvironment.MapPath; }
            set { mapPath = value; }
        }

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

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

        protected internal void WriteLog(int custId, IEnumerable<ITicket> tickets)
            => MonitorService.WriteLog(custId, tickets.ToList());
    }
}