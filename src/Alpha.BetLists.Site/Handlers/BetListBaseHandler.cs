namespace Alpha.BetLists.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Alpha.BetLists.Site.Requests.WinLoss;
    using Alpha.BetLists.Site.ViewModels.Components;
    using Alpha.IPInfo.Services;
    using BetLists.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.Logging.Extensions.Web;
    using SmartPunter.Services;
    using Utilities.IP;

    public class BetListBaseHandler
    {
        private Func<string, string> mapPath;

        public BetListBaseHandler(
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper,
            ISmartPunterBetListMonitor monitorService,
            IPService ipService,
            IIPInfoService ipInfoService)
        {
            Builder = builder;
            TicketHelper = ticketHelper;
            MonitorService = monitorService;
            IpService = ipService;
            IpInfoService = ipInfoService;
            TicketHelper.IsVnIp = IpService.IsVNIP;
            GetOddsTypeName = ticketHelper.GetOddsTypeNameById;
        }

        protected internal IBetListHTMLBuilder Builder { get; }

        protected internal ITicketHelper TicketHelper { get; }

        protected internal ISmartPunterBetListMonitor MonitorService { get; }

        protected internal IPService IpService { get; }

        protected internal IIPInfoService IpInfoService { get; }

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

        protected internal static IEnumerable<ProductViewModel> BuildProductViewModels(
            IEnumerable<Product> products,
            WinLossNewQuery message)
        {
            var productList = new List<ProductViewModel>();

            if (products?.Any() == true)
            {
                var userSelectedProductIds = message.UserSelectedProductIds;
                products
                    .GroupBy(product => new { product.Type, product.TypeName })
                    .ToList()
                    .ForEach(item =>
                    {
                        if (item.Key.Type == 0)
                        {
                            productList.AddRange(item.Select(p => new ProductViewModel(
                                p.Id,
                                p.DisplayName,
                                userSelectedProductIds.ContainsKey(p.Id),
                                p.EndOfSupportDate.HasValue && p.EndOfSupportDate < message.HistoricPivotDate,
                                null)));
                        }
                        else
                        {
                            var children = item.Select(p => new ProductViewModel(
                                p.Id,
                                p.DisplayName,
                                userSelectedProductIds.ContainsKey(p.Id),
                                p.EndOfSupportDate.HasValue && p.EndOfSupportDate < message.HistoricPivotDate,
                                null)).ToList();
                            var isGroupProductChecked = message.FromDate >= message.HistoricPivotDate
                                ? children.Where(p => !p.OnlyShowHistoric).All(p => p.Checked)
                                : children.All(p => p.Checked);
                            productList.Add(new ProductViewModel(
                                item.Key.Type,
                                item.Key.TypeName,
                                isGroupProductChecked,
                                false,
                                children));
                        }
                    });
            }

            return productList;
        }

        protected internal static IEnumerable<ProductDetailViewModel> BuildProductDetailViewModels(
            IEnumerable<ProductDetail> productDetails,
            IDictionary<int, bool> userSelectedResourceIds)
            => productDetails.Any()
            ? productDetails.Select(p => new ProductDetailViewModel(
                p.ProductId,
                p.ResourceId,
                p.DisplayName,
                userSelectedResourceIds.ContainsKey(p.ResourceId)))
            : Enumerable.Empty<ProductDetailViewModel>();
            
        protected internal async Task AssignCountry(IEnumerable<ITicket> tickets, bool displayVNIP)
        {
            var ipList = tickets
                .Where(t => !string.IsNullOrWhiteSpace(t.IP) && (displayVNIP || !IpService.IsVNIP(t.IP.Trim())))
                .Select(t => t.IP.Trim())
                .Distinct();
            var countryDictionary = await GetCountries(ipList);

            string country;
            foreach (var item in tickets)
            {
                if (!string.IsNullOrWhiteSpace(item.IP)
                    && countryDictionary.TryGetValue(item.IP.Trim(), out country))
                {
                    ((TicketDto)item).Country = country;
                }
            }
        }

        protected internal async Task<IDictionary<string, string>> GetCountries(IEnumerable<string> ipList)
        {
            try
            {
                return await IpInfoService.GetCountries(ipList);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return new Dictionary<string, string>();
        }
    }
}