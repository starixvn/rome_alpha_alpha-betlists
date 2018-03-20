namespace Alpha.BetLists.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetLists.Criteria;
    using BetLists.Models;
    using BetLists.Params;
    using BetLists.Services;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexLibsCollection")]
    public class ProductServiceImplTests
    {
        private const string ProductCacheKey = "Alpha.BetLists.Services.Impl.ProductsForCustomer";

        private readonly FanexCacheFixture fanexCacheFixture;
        private readonly FanexDataFixture fanexDataFixture;
        private readonly ProductServiceImpl productService;
        private readonly CompareLogic compareLogic;
        private readonly Func<int, string> getProductName;

        private readonly IEnumerable<Product> allCustomerProductIds = new List<Product>
        {
            new Product(1, "Product 1", null),
            new Product(2, "Product 2", null),
            new Product(3, "Product 3", null)
        };

        public ProductServiceImplTests(
            FanexCacheFixture fanexCacheFixture,
            FanexDataFixture fanexDataFixture)
        {
            this.fanexCacheFixture = fanexCacheFixture;
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

            productService = new ProductServiceImpl(fanexDataFixture.DynamicRepository, canViewProduct, getProductName);

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
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, DateTime.Now, productIds);

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

            IEnumerable<Product> cachedProducts;
            fanexCacheFixture.CacheStorage.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

            fanexDataFixture.DynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, DateTime.Now, productIds);

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
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, DateTime.Now, productIds);

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

            IEnumerable<Product> cachedProducts;
            fanexCacheFixture.CacheStorage.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

            fanexDataFixture.DynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, DateTime.Now, productIds);

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
                new Product(1, null, true, getProductName),
                new Product(2, null, true, getProductName),
                new Product(5, null, true, getProductName)
            };

            // Act
            var actualProducts = productService.GetProductsByGroupHeadline(param);

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
            var actualProductIds = productService.FilterProductIdsByGroupHeadline(productIds, new GetProductsByGroupHeadlineParam(0, DefaultVariables.CurrentDate));

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
            var actualProductIds = productService.FilterProductIdsByGroupHeadline(productIds, new GetProductsByGroupHeadlineParam(0, DefaultVariables.CurrentDate));

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
            var actualProductIds = productService.FilterProductIdsByGroupHeadline(productIds, new GetProductsByGroupHeadlineParam(0, DefaultVariables.CurrentDate));

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }
    }
}