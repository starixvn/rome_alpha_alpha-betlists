namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Criteria;
    using BetLists.DataAccesses;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Constants;
    using Fanex.Caching;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexLibsCollection")]
    public class ProductServiceImplTests
    {
        private const string ProductCacheKey = "Alpha.BetLists.Services.Impl.ProductsForCustomer";

        private readonly FanexDataFixture fanexDataFixture;
        private readonly ProductServiceImpl productService;
        private readonly ProductDataAccess productDataAccess;
        private readonly CompareLogic compareLogic;
        private readonly Func<int, string> getProductName;

        private readonly IEnumerable<Product> allCustomerProductIds = new List<Product>
        {
            new Product(1, "Product 1", null, 0, 0),
            new Product(2, "Product 2", null, 0, 0),
            new Product(3, "Product 3", null, 0, 0)
        };

        public ProductServiceImplTests(
            FanexDataFixture fanexDataFixture)
        {
            this.fanexDataFixture = fanexDataFixture;

            Func<int, bool> canViewProduct = (productId) => true;
            getProductName = (productId) => $"Product {productId}";

            var data = new List<Product>
            {
               new Product(id: 1, name: null, isEnable: true),
               new Product(id: 2, name: null, isEnable: true),
               new Product(id: 3, name: null, isEnable: false),
               new Product(id: 4, name: null, isEnable: false),
               new Product(id: 5, name: null, isEnable: true),
               new Product(id: 6, name: null, isEnable: false)
           };

            fanexDataFixture.DynamicRepository.Fetch<Product>(Arg.Any<GetProductsByGroupHeadlineCriteria>()).Returns(data);
            productDataAccess = Substitute.For<ProductDataAccess>();

            productService = new ProductServiceImpl(fanexDataFixture.DynamicRepository, fanexDataFixture.CacheService, productDataAccess, canViewProduct, getProductName);

            compareLogic = new CompareLogic();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void FilterProductIdsByCustomer_NullProductIdsAndInvalidCustId_ReturnsEmptyProductIds(int custId)
        {
            // Arrange
            IEnumerable<int> productIds = null;
            var expectedProductIds = Enumerable.Empty<int>();

            // Act
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, string.Empty, 1, DateTime.Now, productIds, false);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void FilterProductIdsByCustomer_NullProductIdsAndValidCustId_ReturnsAllCustomerProductIds(int custId)
        {
            // Arrange
            IEnumerable<int> productIds = null;
            var expectedProductIds = allCustomerProductIds.Select(product => product.Id);
            fanexDataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Product>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<Product>>>(1);
                return func();
            });
            fanexDataFixture.DynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, string.Empty, 1, DateTime.Now, productIds, false);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void FilterProductIdsByCustomer_EmptyProductIdsAndInvalidCustId_ReturnsEmptyProductIds(int custId)
        {
            // Arrange
            var productIds = Enumerable.Empty<int>();
            var expectedProductIds = Enumerable.Empty<int>();

            // Act
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, string.Empty, 1, DateTime.Now, productIds, false);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void FilterProductIdsByCustomer_EmptyProductIdsAndValidCustId_ReturnsAllCustomerProductIds(int custId)
        {
            // Arrange
            var productIds = Enumerable.Empty<int>();
            var expectedProductIds = allCustomerProductIds.Select(product => product.Id);
            fanexDataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Product>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<Product>>>(1);
                return func();
            });
            fanexDataFixture.DynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, string.Empty, 1, DateTime.Now, productIds, false);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void GetProductsByGroupHeadline_Always_ReturnsEnableProducts()
        {
            // Arrange
            var param = new GetProductsByGroupHeadlineParam(custId: 0, minDate: DateTime.MinValue);
            var expectedProducts = new List<Product>
            {
                new Product(1, null, true, getProductName, 0, 0),
                new Product(2, null, true, getProductName, 0, 0),
                new Product(5, null, true, getProductName, 0, 0)
            };

            // Act
            var actualProducts = productService.GetProductsByGroupHeadline(param, false);

            // Assert
            Assert.True(compareLogic.Compare(expectedProducts, actualProducts.ToList()).AreEqual);
        }

        [Fact]
        public void FilterProductIdsByCustomer_NullProductIds_ReturnsCorrectProductIds()
        {
            // Arrange
            IEnumerable<int> productIds = null;
            var expectedProductIds = new List<int> { 1, 2, 5 };

            // Act
            var actualProductIds = productService.FilterProductIdsByGroupHeadline(productIds, 
                new GetProductsByGroupHeadlineParam(0, DefaultVariables.CurrentDate), false);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void FilterProductIdsByCustomer_EmptyProductIds_ReturnsCorrectProductIds()
        {
            // Arrange
            var productIds = Enumerable.Empty<int>();
            var expectedProductIds = new List<int> { 1, 2, 5 };

            // Act
            var actualProductIds = productService.FilterProductIdsByGroupHeadline(productIds, 
                new GetProductsByGroupHeadlineParam(0, DefaultVariables.CurrentDate), false);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void FilterProductIdsByCustomer_HasProductIds_ReturnsCorrectProductIds()
        {
            // Arrange
            var productIds = new List<int> { 1, 3, 5 };
            var expectedProductIds = new List<int> { 1, 5 };

            // Act
            var actualProductIds = productService.FilterProductIdsByGroupHeadline(productIds, 
                new GetProductsByGroupHeadlineParam(0, DefaultVariables.CurrentDate), false);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void CanViewBySite_NonOfferedProduct_ReturnFalse()
        {
            // Arrange
            var site = "IBCINdo";
            var products = new List<ProductBySiteModel>() { new ProductBySiteModel(22), new ProductBySiteModel(23) };
            IEnumerable<ProductBySiteModel> cachedProducts;
            fanexDataFixture.CacheService.TryGet($"Alpha.Reports.Services.Impl.ProductsBySite-{site}", out cachedProducts).Returns(false);

            fanexDataFixture.DynamicRepository.Fetch<ProductBySiteModel>(Arg.Any<GetOfferedProductsBySiteCriteria>()).Returns(products);

            // Act
            var act = productService.CanViewBySite(1, site, 1, 21);

            // Assert
            Assert.False(act);
        }

        [Fact]
        public void CanViewBySite_OfferedProduct_ReturnTrue()
        {
            // Arrange
            var site = "IBCINdo";
            var products = new List<ProductBySiteModel>() { new ProductBySiteModel(22), new ProductBySiteModel(23) };
            fanexDataFixture.CacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<ProductBySiteModel>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<ProductBySiteModel>>>(1);
                return func();
            });
            fanexDataFixture.DynamicRepository.Fetch<ProductBySiteModel>(Arg.Any<GetOfferedProductsBySiteCriteria>()).Returns(products);

            // Act
            var act = productService.CanViewBySite(1, site, 1, 23);

            // Assert
            Assert.True(act);
        }

        [Fact]
        public void GetProductsForCustomer_EnableSabaPromotion_ReturnSabaProtionProduct()
        {
            // Arrange

            // Act
            var act = productService.GetProductsForCustomer(1, string.Empty, 1, DateTime.MinValue, isEnableSabaPromotion: true);

            // Assert
            Assert.Equal(37, act.First().Id);
        }

        [Fact]
        public void GetProductsByGroupHeadline_EnableSabaPromotion_ReturnSabaProtionProduct()
        {
            // Arrange
            var param = new GetProductsByGroupHeadlineParam(1, DateTime.MinValue);

            // Act
            var act = productService.GetProductsByGroupHeadline(param, isEnableSabaPromotion: true);

            // Assert
            Assert.Equal(37, act.First().Id);
        }
    }
}