namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Params;

    public interface ProductService
    {
        IEnumerable<Product> GetProductsForCustomer(int custId);

        IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds);

        IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param);
    }

    public class ProductServiceImpl : ProductService
    {
        private const int CustProductsCacheTimeInMinutes = 5;
        private const string ProductsCacheKey = "Alpha.BetLists.Services.Impl.ProductServiceImpl.Products";
        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;

        public ProductServiceImpl(
            IDynamicRepository dynamicRepository,
            Func<int, bool> canViewProduct,
            Func<int, string> getProductName)
        {
            this.dynamicRepository = dynamicRepository;
            this.canViewProduct = canViewProduct;
            this.getProductName = getProductName;
        }

        public IEnumerable<Product> GetProductsForCustomer(int custId)
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
                        .Select(product => new Product(product.Id, product.Name, null, getProductName)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductsCacheTimeInMinutes)));

            return custProducts;
        }

        public IEnumerable<int> FilterProductIdsByCustomer(int custId, IEnumerable<int> productIds)
        {
            var customerProductIds = GetProductsForCustomer(custId).Select(product => product.Id);

            var productIdList = productIds == null || !productIds.Any()
                ? customerProductIds
                : customerProductIds.Where(product => productIds.Contains(product));

            return productIdList;
        }

        public IEnumerable<Product> GetProductsByGroupHeadline(GetProductsByGroupHeadlineParam param)
        {
            var data = dynamicRepository.Fetch<Product>(new GetProductsByGroupHeadlineCriteria(param.CustId))
                .Where(product => IsRemainSupportProduct(product, param.MinDate))
                .Select(product => new Product(product.Id, product.Name, product.IsEnable, getProductName));

            return data;
        }

        public IEnumerable<int> FilterProductIdsByGroupHeadline(IEnumerable<int> productIds, GetProductsByGroupHeadlineParam param)
            => GetProductsByGroupHeadline(param)
            .Select(product => product.Id)
            .Where(id => productIds == null || !productIds.Any() || productIds.Contains(id));

        private static bool IsRemainSupportProduct(Product product, DateTime minReportDate)
            => product.IsEnable
            && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

        private IEnumerable<Product> GetProductsForReport()
        {
            var products = ConfiguredCacheService
                .GetOrSet(
                ProductsCacheKey,
                () => dynamicRepository.Fetch<Product>(new GetProductsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

            return products;
        }
    }
}