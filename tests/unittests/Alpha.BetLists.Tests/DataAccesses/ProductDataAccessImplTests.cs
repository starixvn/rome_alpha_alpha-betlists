namespace Alpha.BetLists.Tests.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using BetLists.Criteria.ProgressiveJackpot;
    using BetLists.DataAccesses;
    using Fanex.Data;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "ProgressiveJackpot")]
    public class ProductDataAccessImplTests
    {
        private readonly CompareLogic compareLogic;
        private readonly IObjectDb objectDb;
        private readonly ProductDataAccessImpl productDataAccess;

        public ProductDataAccessImplTests()
        {
            compareLogic = new CompareLogic();
            DbSettingProviderManager.StartTestingSession().Run();
            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((settingKey, commandTimeout) => objectDb);

            productDataAccess = new ProductDataAccessImpl();
        }

        [Fact]
        public void GetEnabledProductIdsForCustomer_NullCriteria_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => productDataAccess.GetEnabledProductIdsForCustomer(null));
        }

        [Fact]
        public void GetEnabledProductIdsForCustomer_EmptyDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<GetEnableJackpotProductCriteria>()).Returns(dataReader);
            var expectedData = new List<int>();

            // Act
            var actualData = productDataAccess.GetEnabledProductIdsForCustomer(new GetEnableJackpotProductCriteria(1, false));

            // Assert
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetEnabledProductIdsForCustomer_HasData_ReturnsDefaultData()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("productid", typeof(int));
            dataTable.Columns.Add("SportType", typeof(int));
            dataTable.Columns.Add("BetType", typeof(int));
            dataTable.Rows.Add(6, 208, 2179);
            var dataReader = dataTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<GetEnableJackpotProductCriteria>()).Returns(dataReader);
            var expectedData = new List<int> { 6 };

            // Act
            var actualData = productDataAccess.GetEnabledProductIdsForCustomer(new GetEnableJackpotProductCriteria(1, false));

            // Assert
            Assert.Equal(expectedData, actualData);
        }
    }
}