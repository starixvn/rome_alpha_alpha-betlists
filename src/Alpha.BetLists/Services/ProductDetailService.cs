namespace Alpha.BetLists.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Criteria;
    using Alpha.BetLists.Extensions;
    using Alpha.BetLists.Models;
    using AlphaPlus = AlphaPlus.Configurations;
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public interface ProductDetailService
    {
        Task<string> BuildProductDetailsXMLAsync(IEnumerable<int> userSelectedResourceIds, bool is789y = false);

        Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(string site, int currencyId);

        Task<IEnumerable<int>> FilterResourceIdsByCustomer(string site, int currencyId, string resourceIds);
    }

    public class ProductDetailServiceImpl : ProductDetailService
    {
        private const int ProductDetailsCacheInDay = 1;
        private const string ProductDetailsCacheKey = "Alpha.BetLists.Services.Impl.ProductDetailService.ProductDetails";
        private const int CustProductDetailsCacheInMinutes = 5;

        private readonly Func<int, string> getProductDetailName;
        private readonly Func<string, int, int, bool> canViewProduct;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public ProductDetailServiceImpl(
            Func<int, string> getProductDetailName,
            Func<string, int, int, bool> canViewProduct,
            IDynamicRepository dynamicRepository,
            ICacheService cacheService)
        {
            this.getProductDetailName = getProductDetailName;
            this.canViewProduct = canViewProduct;
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public async Task<IEnumerable<int>> FilterResourceIdsByCustomer(string site, int currencyId, string resourceIds)
            => FilterResourceIdsByCustomer(await GetProductDetailsAsync(site, currencyId), resourceIds);

        public async Task<string> BuildProductDetailsXMLAsync(IEnumerable<int> userSelectedResourceIds, bool is789y = false)
        {
            var validProductDetails = new List<ProductDetail>();

            (await GetAllProductDetailsAsync())
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (is789y)
                    {
                        validProductDetails.AddRange(group.Where(i => !IsSupportProductDetail(i.ProductId)));
                    }

                    validProductDetails.AddRange(group.Where(p => userSelectedResourceIds.Contains(p.ResourceId)));
                });

            return BuildProductDetailsXML(validProductDetails);

            string BuildProductDetailsXML(IEnumerable<ProductDetail> productDetails)
            {
                var xml = new StringBuilder("<ROOT>");
                foreach (var product in productDetails)
                {
                    xml.Append(product.ToString());
                }
                xml.Append("</ROOT>");

                return xml.ToString();
            }
        }

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(string site, int currencyId)
        {
            var details = await cacheService.GetOrSetAsync(
                $"{ProductDetailsCacheKey}-{site}-{currencyId}",
                async () =>
                {
                    var canViewProductDetails = CanViewProductDetailsByProduct(site, currencyId);

                    return GroupByProductDetail(await GetAllProductDetailsAsync())
                    .Where(p => canViewProductDetails.ContainsKey(p.ProductId) && CanViewProductDetailByResourceId(p.ResourceId));
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));

            return details;
        }

        private IEnumerable<ProductDetail> GroupByProductDetail(IEnumerable<ProductDetail> productDetails)
            => productDetails
            .GroupBy(r => r.ResourceId)
            .Select(products =>
            {
                var product = products.FirstOrDefault();
                product.DisplayName = getProductDetailName(product.ResourceId);

                return product;
            })
            .ToList()
            .OrderBy(product => product.ProductId)
            .ThenBy(product => product.Order);

        private IEnumerable<int> FilterResourceIdsByCustomer(IEnumerable<ProductDetail> productDetails, string resourceIds)
        {
            if (IsSelectAllProductDetails())
            {
                return productDetails.Select(p => p.ResourceId);
            }

            var resourceIdsHash = resourceIds.ToIntDictionary();

            return productDetails
                .Where(p => resourceIdsHash.ContainsKey(p.ResourceId))
                .Select(p => p.ResourceId);

            bool IsSelectAllProductDetails()
            => resourceIds != null && resourceIds == DefaultVariables.AllProductDetails;
        }

        private IDictionary<int, bool> CanViewProductDetailsByProduct(string site, int currencyId)
        {
            var canViewProductDetails = new Dictionary<int, bool>();

            foreach (var product in AppSettings.ProductOfferProductDetail)
            {
                if (canViewProduct(site, currencyId, product))
                {
                    canViewProductDetails[product] = true;
                }
            }

            return canViewProductDetails;
        }

        private static bool CanViewProductDetailByResourceId(int resourceId)
        {
            if (AlphaPlus.AppSettings.IsSiteLabel777)
            {
                return !ProductDetailIdConstants.ExcludedProductDetailFor777.Contains(resourceId);
            }

            return true;
        }

        private Task<IEnumerable<ProductDetail>> GetAllProductDetailsAsync()
            => cacheService.GetOrSetAsync(
                ProductDetailsCacheKey,
                () => dynamicRepository.FetchAsync<ProductDetail>(new GetProductsDetailCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

        private static bool IsSupportProductDetail(int productId)
            => AppSettings.ProductOfferProductDetail.Any(i => i == productId);
    }
}