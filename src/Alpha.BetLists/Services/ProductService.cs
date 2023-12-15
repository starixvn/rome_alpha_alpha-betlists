namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.BetLists.DataAccesses;
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
        IEnumerable<Product> GetProductsForCustomer(int viewerId, string site, int currencyId, DateTime minReportDate, bool isEnableSabaPromotion);

        IEnumerable<int> FilterProductIdsByCustomer(int viewerId, string site, int currencyId, DateTime minReportDate, IEnumerable<int> productIds, bool isEnableSabaPromotion);

        IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param, bool isEnableSabaPromotion);

        string ConcatProductNames(string productIds);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

        bool IsEnabledJackpot(string site, int currencyId);

        IEnumerable<Product> GetProductsHaveSupportCancelledBet(int viewerId,
            string site,
            int currencyId,
            DateTime minReportDate,
            IEnumerable<int> productIds = null,
            bool isEnableSabaPromotion = false);

        bool CanViewBySite(int viewerId, string site, int currencyId, int productId);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const int CacheDayforJackpot = 7;
        private const string ProductsCacheKey = "Alpha.BetLists.Services.Impl.ProductServiceImpl.Products-{0}-{1}-{2}";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private const int SabaPromotionProductId = 37;
        private readonly ProductDataAccess productDataAccess;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            ProductDataAccess productDataAccess,
            ICacheService cacheService,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.productDataAccess = productDataAccess;
            this.cacheService = cacheService;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
        }

        public IEnumerable<Product> GetProductsForCustomer(int viewerId, string site, int currencyId, DateTime minReportDate, bool isEnableSabaPromotion)
        {
            if (viewerId <= 0)
            {
                return Enumerable.Empty<Product>();
            }

            var custProducts = cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{viewerId}-{site}-{currencyId}",
                () => GetProductsForReport(viewerId, site, currencyId)
                        .Where(product => canViewProduct(product.Id) || CanViewBySite(viewerId, site, currencyId, product.Id))
                        .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, getProductName, product.Type, product.OrderNo)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return GetProductsForCustomer(custProducts, minReportDate, isEnableSabaPromotion);
        }

        private IEnumerable<Product> GetProductsForCustomer(IEnumerable<Product> custProducts, DateTime minReportDate, bool isEnableSabaPromotion)
        {
            if (custProducts != null)
            {
                custProducts = custProducts.Where(product => IsRemainSupportProduct(product, minReportDate));

                if (isEnableSabaPromotion)
                {
                    custProducts = new[]
                    {
                        new Product(SabaPromotionProductId, null, true, getProductName, 0, 0)
                    }.Concat(custProducts);
                }
            }

            return custProducts;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int viewerId, string site, int currencyId, DateTime minReportDate, IEnumerable<int> productIds, bool isEnableSabaPromotion)
        {
            var customerProductIds = GetProductsForCustomer(viewerId, site, currencyId, minReportDate, isEnableSabaPromotion).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> GetProductsHaveSupportCancelledBet(int viewerId,
            string site,
            int currencyId,
            DateTime minReportDate,
            IEnumerable<int> productIds = null,
            bool isEnableSabaPromotion = false)
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

            var allProducts = GetProductsForCustomer(viewerId, site, currencyId, minReportDate, isEnableSabaPromotion).Where(i => supportedProducts.Contains(i.Id));

            var validProducts = productIds == null || !productIds.Any()
                ? allProducts
                : allProducts.Where(product => productIds.Contains(product.Id));

            return validProducts;
        }

        public IEnumerable<Product> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param, bool isEnableSabaPromotion)
        {
            var products = productDataAccess.GetProductsByGroupHeadline(param);
            Logger.Log.Info("Products:\n" + JsonConvert.SerializeObject(products, Formatting.Indented));
            var data = products
                .Where(product => IsRemainSupportProductForGroupHeadline(product, param.MinDate))
                .Select(product => new Product(product.Id, product.Name, product.IsEnable, getProductName, product.Type, product.OrderNo));

            if (isEnableSabaPromotion)
            {
                var sabaProduct = new Product(SabaPromotionProductId, null, true, getProductName, 0, 0);
                var sabaProducts = new List<Product> { sabaProduct };
                data = sabaProducts.Concat(data);
            }

            return data;
        }

        public IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param, bool isEnableSabaPromotion)
            => GetProductsByGroupHeadline(param, isEnableSabaPromotion)
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

        public bool CanViewBySite(int viewerId, string site, int currencyId, int productId)
        {
            var products = cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsBySite-{viewerId}-{site}-{currencyId}",
                () => dynamicRepository.Fetch<ProductBySiteModel>(new GetOfferedProductsBySiteCriteria(viewerId, site, currencyId)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return products.Any(i => i.ProductId == productId);
        }

        private static bool IsRemainSupportProductForGroupHeadline(Product product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        private static bool IsRemainSupportProduct(Product product, DateTime minReportDate)
           => !product.EndOfSupportDate.HasValue
           || minReportDate <= product.EndOfSupportDate.Value;

        private IEnumerable<Product> GetProductsForReport(int viewerId, string site, int currencyId)
        {
            var products = cacheService
                .GetOrSet(
                string.Format(ProductsCacheKey, viewerId, site, currencyId),
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return products;
        }

        private (IEnumerable<JackpotOfferedBySite>, IEnumerable<JackpotOfferedByCurrency>) GetEnabledJackpotProductIdsForCustomer()
            => cacheService.GetOrSet(
                "Alpha.BetLists.Services.Impl.ProductServiceImpl.OfferedJackpot",
                () => dynamicRepository.FetchMultiple<JackpotOfferedBySite, JackpotOfferedByCurrency>(new GetEnableJackpotProductCriteria()),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(CacheDayforJackpot)));
    }
}