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
        IEnumerable<Product> GetProductsForCustomer(int custId, DateTime minReportDate);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, DateTime minReportDate, IEnumerable<int> productIds);

        IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param);

        string ConcatProductNames(string productIds);

        IEnumerable<string> GetProductNames(IEnumerable<int> productIds);

        bool IsEnabledJackpot(int custId);

        IEnumerable<Product> GetProductsHaveSupportCancelledBet(int custId, DateTime minReportDate, IEnumerable<int> productIds = null);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const string ProductsCacheKey = "Alpha.BetLists.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ProductDataAccess productDataAccess;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            ProductDataAccess productDataAccess,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.productDataAccess = productDataAccess;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId, DateTime minReportDate)
        {
            if (custId <= 0)
            {
                return Enumerable.Empty<Product>();
            }

            var custProducts = ConfiguredCacheService
                .GetOrSet(
                $"Alpha.Reports.Services.Impl.ProductsForCustomer-{custId}",
                () => GetProductsForReport()
                        .Where(product => canViewProduct(product.Id))
                        .Select(product => new Product(product.Id, product.Name, product.EndOfSupportDate, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            if (custProducts != null)
            {
                custProducts = custProducts.Where(product => IsRemainSupportProduct(product, minReportDate));
            }

            return custProducts;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, DateTime minReportDate, IEnumerable<int> productIds)
        {
            var customerProductIds = GetProductsForCustomer(custId, minReportDate).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> GetProductsHaveSupportCancelledBet(int custId, DateTime minReportDate, IEnumerable<int> productIds = null)
        {
            var supportedProducts = new List<int>
            {
                ProductIdConstants.Sportsbook,
                ProductIdConstants.NumberGame,
                ProductIdConstants.VirtualSports,
                ProductIdConstants.VirtualSports2,
                ProductIdConstants.Keno,
                ProductIdConstants.GoldDeluxe,
                ProductIdConstants.ColossusBets,
                ProductIdConstants.NG2,
                ProductIdConstants.Lottery,
                ProductIdConstants.Bullfighting,
                ProductIdConstants.PokerGames
            };

            var allProducts = GetProductsForCustomer(custId, minReportDate).Where(i => supportedProducts.Contains(i.Id));

            var validProducts = productIds == null || !productIds.Any()
                ? allProducts
                : allProducts.Where(product => productIds.Contains(product.Id));

            return validProducts;
        }

        public IEnumerable<Product> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param)
        {
            var data = dynamicRepository.Fetch<Product>(new GetProductsByGroupHeadlineCriteria(param.CustId))
                .Where(product => IsRemainSupportProductForGroupHeadline(product, param.MinDate))
                .Select(product => new Product(product.Id, product.Name, product.IsEnable, getProductName));

            return data;
        }

        public IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param)
            => GetProductsByGroupHeadline(param)
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

        private static bool IsRemainSupportProductForGroupHeadline(Product product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        private static bool IsRemainSupportProduct(Product product, DateTime minReportDate)
           => !product.EndOfSupportDate.HasValue
           || minReportDate <= product.EndOfSupportDate.Value;

        private IEnumerable<Product> GetProductsForReport()
        {
            var products = ConfiguredCacheService
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
