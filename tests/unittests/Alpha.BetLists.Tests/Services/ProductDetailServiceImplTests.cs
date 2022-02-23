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

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, ProductIdConstants.Pragmatic, 200, 0, "0", "0"),
                    new ProductDetail(2, ProductIdConstants.Pragmatic, 150, 0, "0", "0"),
                    new ProductDetail(3, ProductIdConstants.Sportsbook, 200, 0, "0", "0"),
                    new ProductDetail(4, ProductIdConstants.Sportsbook, 150, 0, "0", "0")
                });
        }

        [Fact]
        public async Task BuildProductDetailsXML_OfferSB_ReturnsSBOnlyXML()
        {
            // Arrange
            IEnumerable<int> validProductIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = "3,4";

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("ProductOfferProductDetail", "1");

            // Act
            var actualResult = await productDetailService.BuildProductDetailsXMLAsync(validProductIds, userSelectedResourceIds);

            // Assert
            Assert.Equal(
                $"<ROOT><ProductDetail ProductId=\"{ProductIdConstants.Sportsbook}\" Sporttype=\"200\" Bettype=\"0\" LeagueGroupID=\"0\" TicketType=\"0\"/>"
                + $"<ProductDetail ProductId=\"{ProductIdConstants.Sportsbook}\" Sporttype=\"150\" Bettype=\"0\" LeagueGroupID=\"0\" TicketType=\"0\"/></ROOT>",
                actualResult);
        }

        [Fact]
        public async Task BuildProductDetailsXML_OfferAll_ReturnsSBAndPramaticProductDetailsXML()
        {
            // Arrange
            IEnumerable<int> validProductIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = "1,2,3,4";

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("ProductOfferProductDetail", "1,32");

            // Act
            var actualResult = await productDetailService.BuildProductDetailsXMLAsync(validProductIds, userSelectedResourceIds);

            // Assert
            Assert.Equal(
                $"<ROOT><ProductDetail ProductId=\"{ProductIdConstants.Pragmatic}\" Sporttype=\"200\" Bettype=\"0\" LeagueGroupID=\"0\" TicketType=\"0\"/>"
                + $"<ProductDetail ProductId=\"{ProductIdConstants.Pragmatic}\" Sporttype=\"150\" Bettype=\"0\" LeagueGroupID=\"0\" TicketType=\"0\"/>"
                + $"<ProductDetail ProductId=\"{ProductIdConstants.Sportsbook}\" Sporttype=\"200\" Bettype=\"0\" LeagueGroupID=\"0\" TicketType=\"0\"/>"
                + $"<ProductDetail ProductId=\"{ProductIdConstants.Sportsbook}\" Sporttype=\"150\" Bettype=\"0\" LeagueGroupID=\"0\" TicketType=\"0\"/></ROOT>",
                actualResult);
        }

        [Fact]
        public async Task RemoveProductIdsAsync_UserSelectedResourceIdsHaveValues_ReturnsProductIds()
        {
            // Arrange
            IEnumerable<int> productIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = "1,2,3,4";

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("ProductOfferProductDetail", "1,32");

            dynamicRepository
                .FetchAsync<ProductDetail>(Arg.Any<GetProductsDetailCriteria>())
                .Returns(new List<ProductDetail>
                {
                    new ProductDetail(1, ProductIdConstants.Pragmatic, 200, 0, "0", "0"),
                    new ProductDetail(2, ProductIdConstants.Pragmatic, 150, 0, "0", "0")
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
            var userSelectedResourceIds = "1";

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("ProductOfferProductDetail", "1,32");

            // Act
            var actualResult = await productDetailService.RemoveProductIdsAsync(productIds, userSelectedResourceIds);

            // Assert
            Assert.True(string.Join(",", actualResult) == "32");
        }

        [Fact]
        public async Task RemoveProductIdsAsync_NotHaveProductDetail_ReturnsProductIds()
        {
            // Arrange
            IEnumerable<int> productIds = new List<int> { 1, ProductIdConstants.Pragmatic };
            var userSelectedResourceIds = string.Empty;

            AppSettings.AppSettingValues = new NameValueCollection();
            AppSettings.AppSettingValues.Add("ProductOfferProductDetail", "");

            // Act
            var actualResult = await productDetailService.RemoveProductIdsAsync(productIds, userSelectedResourceIds);

            // Assert
            Assert.True(string.Join(",", actualResult) == string.Join(",", productIds));
        }
    }
}
