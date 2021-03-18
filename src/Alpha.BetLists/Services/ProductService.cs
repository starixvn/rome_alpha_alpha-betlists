namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Constants;
    using Criteria;
    using Criteria.ProgressiveJackpot;
    using DataAccesses;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface ProductService
    {
        IEnumerable<Product> GetProductsForCustomer(int custId, string site, int currencyId, DateTime minReportDate, bool isEnableSabaPromotion);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, string site, int currencyId, DateTime minReportDate, IEnumerable<int> productIds, bool isEnableSabaPromotion);

        IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param, bool isEnableSabaPromotion);

        string ConcatProductNames(string productIds);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

        bool IsEnabledJackpot(int custId);

        IEnumerable<Product> GetProductsHaveSupportCancelledBet(int custId, 
            string site, 
            int currencyId, 
            DateTime minReportDate, 
            IEnumerable<int> productIds = null, 
            bool isEnableSabaPromotion = false);

        bool CanViewBySite(int custId, string site, int currencyId, int productId);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const string ProductsCacheKey = "Alpha.BetLists.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private readonly ProductDataAccess productDataAccess;
        private const int SabaPromotionProductId = 37;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            ProductDataAccess productDataAccess,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.productDataAccess = productDataAccess;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId, string site, int currencyId, DateTime minReportDate, bool isEnableSabaPromotion)
        {
            if (custId <= 0)
            {
                return Enumerable.Empty<Product>();
            }

            var custProducts = cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}",
                () => GetProductsForReport()
                        .Where(product => canViewProduct(product.Id) || CanViewBySite(custId, site, currencyId, product.Id))
                        .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, getProductName, product.Type, product.OrderNo)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            if (custProducts != null)
            {
                custProducts = custProducts.Where(product => IsRemainSupportProduct(product, minReportDate));
            }

            if (isEnableSabaPromotion)
            {
                var sabaProduct = new Product(SabaPromotionProductId, null, true, getProductName, 0, 0);
                var sabaProducts = new List<Product> { sabaProduct };
                custProducts = sabaProducts.Concat(custProducts);
            }

            return custProducts;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, string site, int currencyId, DateTime minReportDate, IEnumerable<int> productIds, bool isEnableSabaPromotion)
        {
            var customerProductIds = GetProductsForCustomer(custId, site, currencyId, minReportDate, isEnableSabaPromotion).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> GetProductsHaveSupportCancelledBet(int custId, 
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
                ProductIdConstants.NG2,
                ProductIdConstants.Lottery,
                ProductIdConstants.TableGames,
                ProductIdConstants.VirtualGames,
                ProductIdConstants.SportsLottery
            };

            var allProducts = GetProductsForCustomer(custId, site, currencyId, minReportDate, isEnableSabaPromotion).Where(i => supportedProducts.Contains(i.Id));

            var validProducts = productIds == null || !productIds.Any()
                ? allProducts
                : allProducts.Where(product => productIds.Contains(product.Id));

            return validProducts;
        }

        public IEnumerable<Product> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param, bool isEnableSabaPromotion)
        {
            var data = dynamicRepository.Fetch<Product>(new GetProductsByGroupHeadlineCriteria(param.CustId))
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

        public bool IsEnabledJackpot(int custId)
        {
            var productIds = GetEnabledJackpotProductIdsForCustomer(custId);

            return productIds.Any();
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

        public bool CanViewBySite(int custId, string site, int currencyId, int productId)
        {
            var products = cacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsBySite-{custId}-{site}",
                () => dynamicRepository.Fetch<ProductBySiteModel>(new GetOfferedProductsBySiteCriteria(custId, site, currencyId)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return products.Any(i => i.ProductId == productId);
        }

        private static bool IsRemainSupportProductForGroupHeadline(Product product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        private static bool IsRemainSupportProduct(Product product, DateTime minReportDate)
           => !product.EndOfSupportDate.HasValue
           || minReportDate <= product.EndOfSupportDate.Value;

        private IEnumerable<Product> GetProductsForReport()
        {
            var products = cacheService
                .GetOrSet(
                ProductsCacheKey,
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return products;
        }

        private IEnumerable<int> GetEnabledJackpotProductIdsForCustomer(int custId)
           => productDataAccess.GetEnabledProductIdsForCustomer(new GetEnableJackpotProductCriteria(custId, AppSettings.LocalTest));
    }
}