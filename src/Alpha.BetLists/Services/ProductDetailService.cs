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
    using Fanex.Caching;
    using Fanex.Data.Repository;

    public interface ProductDetailService
    {
        Task<string> BuildProductDetailsXMLAsync(IEnumerable<int> validProductIds, string userSelectedResourceIds, bool is789y = false);

        Task<IEnumerable<int>> RemoveProductIdsAsync(IEnumerable<int> productIds, string userSelectedResourceIds);

        Task<IEnumerable<int>> RemoveProductIdsAsync(IEnumerable<int> productIds, IEnumerable<int> resourceIds);

        Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(int viewerId, string site, int currencyId);
    }

    public class ProductDetailServiceImpl : ProductDetailService
    {
        private const int ProductDetailsCacheInDay = 1;
        private const string ProductDetailsCacheKey = "Alpha.BetLists.Services.Impl.ProductDetailService.ProductDetails";
        private const int CustProductDetailsCacheInMinutes = 5;

        private readonly Func<string, int, string> getProductDetailName;
        private readonly Func<string, int, int, bool> canViewProduct;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public ProductDetailServiceImpl(
            Func<string, int, string> getProductDetailName,
            Func<string, int, int, bool> canViewProduct,
            IDynamicRepository dynamicRepository,
            ICacheService cacheService)
        {
            this.getProductDetailName = getProductDetailName;
            this.canViewProduct = canViewProduct;
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public async Task<string> BuildProductDetailsXMLAsync(IEnumerable<int> validProductIds, string userSelectedResourceIds, bool is789y = false)
        {
            var validProductDetails = new List<ProductDetail>();
            var resourceIdsHash = userSelectedResourceIds.ToIntDictionary();

            (await GetProductDetailsAsync())
                .Where(p => validProductIds.Contains(p.ProductId))
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (is789y)
                    {
                        validProductDetails.AddRange(group.Where(i => !IsSupportProductDetail(i.ProductId)));
                    }

                    validProductDetails.AddRange(group.Where(p => resourceIdsHash.ContainsKey(p.ResourceId)));
                });

            return validProductDetails.Any()
                ? BuildProductDetailsXML(validProductDetails)
                : string.Empty;
        }

        private Task<IEnumerable<ProductDetail>> GetProductDetailsAsync()
            => cacheService.GetOrSetAsync(
                ProductDetailsCacheKey,
                () => dynamicRepository.FetchAsync<ProductDetail>(new GetProductsDetailCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

        private static string BuildProductDetailsXML(IEnumerable<ProductDetail> productDetails)
        {
            var xml = new StringBuilder("<ROOT>");
            foreach (var product in productDetails)
            {
                foreach (var leagueId in product.LeagueGroupIds)
                {
                    foreach (var typeId in product.TicketTypeIds)
                    {
                        xml.AppendFormat(
                            "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\" LeagueGroupID=\"{3}\" TicketType=\"{4}\"/>",
                            product.ProductId,
                            product.SportType,
                            product.BetType,
                            leagueId,
                            typeId);
                    }
                }
            }
            xml.Append("</ROOT>");

            return xml.ToString();
        }

        public Task<IEnumerable<int>> RemoveProductIdsAsync(IEnumerable<int> productIds, string userSelectedResourceIds)
            => RemoveProductIdsAsync(productIds, userSelectedResourceIds.ToListInt());

        public async Task<IEnumerable<int>> RemoveProductIdsAsync(IEnumerable<int> productIds, IEnumerable<int> resourceIds)
        {
            var removedProductIds = new List<int>(productIds);

            (await GetProductDetailsAsync())
                .Where(p => productIds.Contains(p.ProductId))
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (IsSupportProductDetail(group.Key))
                    {
                        var intersectResourceIds = group.Select(p => p.ResourceId).Intersect(resourceIds);

                        if (!intersectResourceIds.Any())
                        {
                            removedProductIds.RemoveAll(id => id == group.Key);
                        }
                    }
                });

            return removedProductIds;
        }

        private static bool IsSupportProductDetail(int productId)
            => AppSettings.ProductOfferProductDetail.Any(i => i == productId);

        public async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync(int viewerId, string site, int currencyId)
        {
            if (viewerId <= 0)
            {
                return Enumerable.Empty<ProductDetail>();
            }

            var canViewProductDetails = CanViewProductDetails(site, currencyId);

            return await cacheService.GetOrSetAsync(
                $"{ProductDetailsCacheKey}-{viewerId}",
                async () => (await GetProductDetailsAsync())
                    .Where(p => canViewProductDetails.ContainsKey(p.ProductId) && SportsbookSupportDetail(p))
                    .Select(product =>
                    {
                        product.DisplayName = getProductDetailName(product.RefId, product.ResourceId);

                        return product;
                    }),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CustProductDetailsCacheInMinutes)));
        }

        private IDictionary<int, bool> CanViewProductDetails(string site, int currencyId)
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

        private static bool SportsbookSupportDetail(ProductDetail detail)
        {
            if (AlphaPlus.Configurations.AppSettings.IsSiteLabel777 && detail.ProductId == ProductIdConstants.Sportsbook)
            {
                return ProductDetailIdConstants.SportsbookDetailFor777Site.Contains(detail.ResourceId);
            }

            return true;
        }
    }
}