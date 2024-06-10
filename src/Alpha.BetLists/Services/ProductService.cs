namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Criteria;
    using Criteria.ProgressiveJackpot;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Models;
    using Newtonsoft.Json;
    using Params;

    public interface ProductService
    {
        IEnumerable<Product> GetAllOfferedProducts(string site, int currencyId, DateTime minReportDate);

        IEnumerable<int> FilterProductIds(string site, int currencyId, DateTime minReportDate, IEnumerable<int> productIds);

        IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param);

        string ConcatProductNames(string productIds);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

        bool IsEnabledJackpot(string site, int currencyId);

        IEnumerable<Product> GetProductsHaveSupportCancelledBet(int viewerId,
            string site,
            int currencyId,
            DateTime minReportDate,
            IEnumerable<int> productIds = null);

        bool CanViewProduct(string site, int currencyId, int productId);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const int CacheDayforJackpot = 7;
        private const string ProductsCacheKey = "Alpha.BetLists.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
        }

        public IEnumerable<Product> GetAllOfferedProducts(string site, int currencyId, DateTime minReportDate)
            => GetOfferedProducts(site, currencyId, minReportDate);

        public IEnumerable<int> FilterProductIds(string site, int currencyId, DateTime minReportDate, IEnumerable<int> productIds)
        {
            var customerProductIds = GetAllOfferedProducts(site, currencyId, minReportDate).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> GetProductsHaveSupportCancelledBet(int viewerId,
            string site,
            int currencyId,
            DateTime minReportDate,
            IEnumerable<int> productIds = null)
        {
            var supportedProducts = new List<int>
            {
                ProductIdConstants.Sportsbook,
                ProductIdConstants.NumberGame,
                ProductIdConstants.VirtualSports,
                ProductIdConstants.VirtualSports2,
                ProductIdConstants.Keno,
                ProductIdConstants.GoldDeluxe,
                ProductIdConstants.Lottery,
                ProductIdConstants.TableGames,
                ProductIdConstants.VirtualGames,
                ProductIdConstants.SportsLottery,
                ProductIdConstants.Bitcoin
            };

            var allProducts = GetAllOfferedProducts(site, currencyId, minReportDate).Where(i => supportedProducts.Contains(i.Id));

            var validProducts = productIds == null || !productIds.Any()
                ? allProducts
                : allProducts.Where(product => productIds.Contains(product.Id));

            return validProducts;
        }

        public IEnumerable<Product> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param)
        {
            //var products = productDataAccess.GetProductsByGroupHeadline(param);
            var products = dynamicRepository.Fetch<CosolidateProduct>(new GetProductsByGroupHeadlineCriteria(param.CustId));
            Logger.Log.Info("Products:\n" + JsonConvert.SerializeObject(products, Formatting.Indented));
            var data = products
                .Where(product
                    => IsRemainSupportProductForGroupHeadline(new Product(product.Id, product.Name, product.EndOfSupportDate, product.IsEnable, product.Type, product.OrderNo), param.MinDate))
                .Select(product => new Product(product.Id, product.Name, product.IsEnable, getProductName, product.Type, product.OrderNo));

            return data;

            bool IsRemainSupportProductForGroupHeadline(Product product, DateTime? minReportDate)
             => !minReportDate.HasValue || !product.EndOfSupportDate.HasValue || minReportDate.Value <= product.EndOfSupportDate.Value;
        }

        public IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param)
            => GetProductsByGroupHeadline(param)
            .Select(product => product.Id)
            .Where(id => productIds == null || !productIds.Any() || productIds.Contains(id));

        public bool IsEnabledJackpot(string site, int currencyId)
        {
            var offeredJackpot = GetEnabledJackpotProductIdsForCustomer();
            var productIdBySite = offeredJackpot.Item1.Where(i => i.Site.Equals(site, StringComparison.CurrentCultureIgnoreCase));
            var productIdByCurrency = offeredJackpot.Item2
                .Where(i => i.Currency == currencyId
                        && productIdBySite.Any(s => s.Product == i.Product));

            return productIdByCurrency.Any();
        }

        public IEnumerable<string> GetProductNames(IEnumerable<int> productIds)
        {
            foreach (var productId in productIds)
            {
                yield return getProductName(productId);
            }
        }

        public string ConcatProductNames(string productIds)
            => string.Join(", ", GetProductNames(productIds.ToListInt()));

        public bool CanViewProduct(string site, int currencyId, int productId)
            => FilterProductIds(site, currencyId, DateTime.MinValue, new List<int> { productId }).Any();

        private (IEnumerable<JackpotOfferedBySite>, IEnumerable<JackpotOfferedByCurrency>) GetEnabledJackpotProductIdsForCustomer()
            => cacheService.GetOrSet(
                "Alpha.BetLists.Services.Impl.ProductServiceImpl.OfferedJackpot",
                () => dynamicRepository.FetchMultiple<JackpotOfferedBySite, JackpotOfferedByCurrency>(new GetEnableJackpotProductCriteria()),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheDayforJackpot)));

        private IEnumerable<Product> GetOfferedProducts(string site, int currencyId, DateTime minReportDate)
        {
            var products = cacheService
                .GetOrSet(
               $"{ProductsCacheKey}--{site}--{currencyId}",
               () => dynamicRepository.Fetch<Product>(new GetProductsCriteria(site, currencyId))
                   .Where(product => !product.IsHide)
                   .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, getProductName, product.Type, product.OrderNo)),
               new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return products.Where(i => IsRemainSupportProduct(i, minReportDate) && canViewProduct(i.Id));
        }

        private static bool IsRemainSupportProduct(Product product, DateTime minReportDate)
            => !product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value;
    }
}