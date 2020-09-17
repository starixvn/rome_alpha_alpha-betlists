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
        Task<string> BuildProductDetailsXMLAsync(IEnumerable<int> validProductIds, string userSelectedResourceIds);

        Task<IEnumerable<int>> RemoveProductIdsAsync(IEnumerable<int> productIds, string userSelectedResourceIds);
    }

    public class ProductDetailServiceImpl : ProductDetailService
    {
        private const int ProductDetailsCacheInDay = 1;
        private const string ProductDetailsCacheKey = "Alpha.BetLists.Services.Impl.ProductDetailService.ProductDetails";

        private readonly IDynamicRepository dynamicRepository;

        public ProductDetailServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<string> BuildProductDetailsXMLAsync(IEnumerable<int> validProductIds, string userSelectedResourceIds)
        {
            var validProductDetails = new List<ProductDetail>();
            var resourceIdsHash = userSelectedResourceIds.ToIntDictionary();

            (await GetProductDetailsAsync())
                .Where(p => validProductIds.Contains(p.ProductId))
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (group.Key == ProductIdConstants.Pragmatic)
                    {
                        validProductDetails.AddRange(
                            AppSettings.IsOfferJackpotProductDetails ? group.Where(p => resourceIdsHash.ContainsKey(p.ResourceId)) : group);
                    }
                });

            return validProductDetails.Any()
                ? BuildProductDetailsXML(validProductDetails)
                : string.Empty;
        }

        private async Task<IEnumerable<ProductDetail>> GetProductDetailsAsync()
            => await ConfiguredCacheService.GetOrSet(
                ProductDetailsCacheKey,
                () => dynamicRepository.FetchAsync<ProductDetail>(new GetProductsDetailCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(ProductDetailsCacheInDay)));

        private static string BuildProductDetailsXML(IEnumerable<ProductDetail> productDetails)
        {
            var xml = new StringBuilder("<ROOT>");
            productDetails.ToList().ForEach(product => xml.AppendFormat(
                "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\"/>",
                product.ProductId,
                product.SportType,
                product.BetType));
            xml.Append("</ROOT>");

            return xml.ToString();
        }

        public async Task<IEnumerable<int>> RemoveProductIdsAsync(IEnumerable<int> productIds, string userSelectedResourceIds)
        {
            var removedProductIds = new List<int>(productIds);

            (await GetProductDetailsAsync())
                .Where(p => productIds.Contains(p.ProductId))
                .GroupBy(p => p.ProductId)
                .ToList()
                .ForEach(group =>
                {
                    if (group.Key == ProductIdConstants.Pragmatic && AppSettings.IsOfferJackpotProductDetails)
                    {
                        var intersectResourceIds = group.Select(p => p.ResourceId).Intersect(userSelectedResourceIds.ToListInt());

                        if (!intersectResourceIds.Any())
                        {
                            removedProductIds.RemoveAll(id => id == group.Key);
                        }
                    }
                });

            return removedProductIds;
        }
    }
}
