namespace Alpha.BetLists.Tests.Services
{
    using BetLists.Criteria;
    using BetLists.Models;
    using BetLists.Services;
    using Fixtures;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexLibsCollection")]
    public class ProductServiceImplTests
    {
        private const string ProductCacheKey = "Alpha.BetLists.Services.Impl.ProductsForCustomer";

        private readonly FanexCacheFixture fanexCacheFixture;
        private readonly FanexDataFixture fanexDataFixture;
        private readonly ProductService productService;

        private readonly IEnumerable<Product> allCustomerProductIds = new List<Product>
        {
            new Product(1, "Product 1"),
            new Product(2, "Product 2"),
            new Product(3, "Product 3")
        };

        public ProductServiceImplTests(
            FanexCacheFixture fanexCacheFixture,
            FanexDataFixture fanexDataFixture)
        {
            this.fanexCacheFixture = fanexCacheFixture;
            this.fanexDataFixture = fanexDataFixture;

            Func<int, bool> canViewProduct = (productId) => true;
            Func<int, string> getProductName = (productId) => $"Product {productId}";

            productService = new ProductServiceImpl(fanexDataFixture.DynamicRepository, canViewProduct, getProductName);
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
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, productIds);

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
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, productIds);

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
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, productIds);

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
            var actualProductIds = productService.FilterProductIdsByCustomer(custId, productIds);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }
    }
}