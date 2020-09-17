namespace Alpha.BetLists.Tests.Services
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading.Tasks;
    using Alpha.BetLists.Configurations;
    using Alpha.BetLists.Constants;
    using Alpha.BetLists.Criteria;
    using Alpha.BetLists.Models;
    using Alpha.BetLists.Services;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    public class ProductDetailServiceImplTests
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ProductDetailService productDetailService;

        public ProductDetailServiceImplTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            productDetailService = new ProductDetailServiceImpl(dynamicRepository);
        }

        [Fact]
        public async Task BuildProductDetailsXML_IsOfferJackpotProductDetailsHasProductId_ReturnsCorrectXML()
        {
            // Arrange
            IEnumerable<int> validProductIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = "1,2";

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("IsOfferJackpotProductDetails", "1");

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, ProductIdConstants.Pragmatic, 200, 0),
                    new ProductDetail(2, ProductIdConstants.Pragmatic, 150, 0)
                });

            // Act
            var actualResult = await productDetailService.BuildProductDetailsXMLAsync(validProductIds, userSelectedResourceIds);

            // Assert
            Assert.Equal(
                $"<ROOT><ProductDetail ProductId=\"{ProductIdConstants.Pragmatic}\" Sporttype=\"200\" Bettype=\"0\"/>"
                + $"<ProductDetail ProductId=\"{ProductIdConstants.Pragmatic}\" Sporttype=\"150\" Bettype=\"0\"/></ROOT>",
                actualResult);
        }

        [Fact]
        public async Task BuildProductDetailsXML_IsOfferJackpotProductDetailsProductIdIsEmpty_ReturnsEmptyXML()
        {
            // Arrange
            IEnumerable<int> validProductIds = new List<int>();
            var userSelectedResourceIds = "1,2";

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("SiteGroup", "789y");

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, 42, 200, 0),
                    new ProductDetail(1, 42, 150, 0)
                });

            // Act
            var actualResult = await productDetailService.BuildProductDetailsXMLAsync(validProductIds, userSelectedResourceIds);

            // Assert
            Assert.Empty(actualResult);
        }

        [Fact]
        public async Task BuildProductDetailsXML_IsNotOfferJackpotProductDetailsHasProductId_ReturnsAllProductDetailsXML()
        {
            // Arrange
            IEnumerable<int> validProductIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = string.Empty;

            AppSettings.AppSettingValues = new NameValueCollection();

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, ProductIdConstants.Pragmatic, 200, 0),
                    new ProductDetail(2, ProductIdConstants.Pragmatic, 150, 0)
                });

            // Act
            var actualResult = await productDetailService.BuildProductDetailsXMLAsync(validProductIds, userSelectedResourceIds);

            // Assert
            Assert.Equal(
                $"<ROOT><ProductDetail ProductId=\"{ProductIdConstants.Pragmatic}\" Sporttype=\"200\" Bettype=\"0\"/>"
                + $"<ProductDetail ProductId=\"{ProductIdConstants.Pragmatic}\" Sporttype=\"150\" Bettype=\"0\"/></ROOT>",
                actualResult);
        }

        [Fact]
        public async Task RemoveProductIdsAsync_UserSelectedResourceIdsHaveValues_ReturnsProductIds()
        {
            // Arrange
            IEnumerable<int> productIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = "1,2";

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("IsOfferJackpotProductDetails", "1");

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, ProductIdConstants.Pragmatic, 200, 0),
                    new ProductDetail(2, ProductIdConstants.Pragmatic, 150, 0)
                });

            // Act
            var actualResult = await productDetailService.RemoveProductIdsAsync(productIds, userSelectedResourceIds);

            // Assert
            Assert.True(string.Join(",", actualResult) == string.Join(",", productIds));
        }

        [Fact]
        public async Task RemoveProductIdsAsync_UserSelectedResourceIdsDontHaveValues_ReturnsProductIds()
        {
            // Arrange
            IEnumerable<int> productIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = string.Empty;

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("IsOfferJackpotProductDetails", "1");

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, ProductIdConstants.Pragmatic, 200, 0),
                    new ProductDetail(2, ProductIdConstants.Pragmatic, 150, 0)
                });

            // Act
            var actualResult = await productDetailService.RemoveProductIdsAsync(productIds, userSelectedResourceIds);

            // Assert
            Assert.True(string.Join(",", actualResult) == "1");
        }

        [Fact]
        public async Task RemoveProductIdsAsync_IsNotOfferJackpotProductDetails_ReturnsProductIds()
        {
            // Arrange
            IEnumerable<int> productIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = string.Empty;

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("IsOfferJackpotProductDetails", "0");

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, ProductIdConstants.Pragmatic, 200, 0),
                    new ProductDetail(2, ProductIdConstants.Pragmatic, 150, 0)
                });

            // Act
            var actualResult = await productDetailService.RemoveProductIdsAsync(productIds, userSelectedResourceIds);

            // Assert
            Assert.True(string.Join(",", actualResult) == string.Join(",", productIds));
        }
    }
}
