namespace Alpha.BetLists.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.ViewModels.Components;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using SmartPunter.Services;
    using Utilities.IP;

    public class BetListBaseHandler
    {
        private Func<string, string> mapPath;

        public BetListBaseHandler(
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService)
        {
            Builder = builder;
            TicketHelper = ticketHelper;
            MonitorService = monitorService;
            IpService = ipService;
            TicketHelper.IsVnIp = IpService.IsVNIP;
            GetOddsTypeName = ticketHelper.GetOddsTypeNameById;
        }

        protected internal IBetListHTMLBuilder Builder { get; }

        protected internal ITicketHelper TicketHelper { get; }

        protected internal ISmartPunterBetListMonitor MonitorService { get; }

        protected internal IPService IpService { get; }

        protected internal GetCachePropertyById GetOddsTypeName { get; }

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HostingEnvironment.MapPath; }
            set { mapPath = value; }
        }

        protected internal void WriteLog(int custId, IEnumerable<TicketDto> tickets)
             => MonitorService.WriteLog(custId, tickets.ToList<ITicket>());

        protected internal IEnumerable<GroupProductViewModel> BuildProductSelectListItems(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products)
            => products?.Any() == true
                ? products
                    .GroupBy(product => new { product.Type, product.TypeName })
                    .Select(grp => new GroupProductViewModel(
                        new SelectListGroup { Name = grp.Key.TypeName },
                        grp.Select(product => new SelectListItem
                        {
                            Text = product.DisplayName,
                            Value = product.Id.ToString(),
                            Selected = userSelectedProductIds.Contains(product.Id)
                        })))
                : Enumerable.Empty<GroupProductViewModel>();
    }
}