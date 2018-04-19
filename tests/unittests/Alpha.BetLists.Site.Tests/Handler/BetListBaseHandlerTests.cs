namespace Alpha.BetLists.Site.Tests.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using BetLists.Models;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fixtures;
    using Handlers;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class BetListBaseHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly BetListBaseHandler baseHandler;
        private readonly WinLossQuery query;
        private readonly WinLossViewModel viewModel;
        private readonly CompareLogic compareLogic;

        public BetListBaseHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            baseHandler = new BetListBaseHandler(
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MonitorService,
                this.betlistFixture.IpService);

            query = new WinLossQuery(betlistFixture.User);
            viewModel = new WinLossViewModel(query, BetListDataTest.BetListData, betlistFixture.HtmlBuilder, betlistFixture.TicketHelper, false);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void Builder_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.Builder, betlistFixture.HtmlBuilder);
        }

        [Fact]
        public void TicketHelper_Always_SetValueFromCtor()
        {
            // Act && Assert
            Assert.Equal(baseHandler.TicketHelper, betlistFixture.TicketHelper);
        }

        [Fact]
        public void TicketHelper_Always_SetIsVnIpFromIpService()
        {
            // Act & Assert
            Assert.Equal(baseHandler.TicketHelper.IsVnIp, betlistFixture.IpService.IsVNIP);
        }

        [Fact]
        public void MonitorService_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.MonitorService, betlistFixture.MonitorService);
        }

        [Fact]
        public void IpService_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.IpService, betlistFixture.IpService);
        }

        [Fact]
        public void GetOddsTypeName_Always_ReturnDefaultValue()
        {
            // Act & Assert
            Assert.Equal(CacheRef.GetOddsTypeName, baseHandler.GetOddsTypeName);
        }

        [Fact]
        public void MapPath_WhenMapPathIsNotNull_ReturnMapPathValue()
        {
            // Arrange
            Func<string, string> mapPath = (path) => "path";

            // Act
            baseHandler.MapPath = mapPath;

            // Assert
            Assert.Equal(baseHandler.MapPath, mapPath);
        }

        [Fact]
        public void MapPath_WhenMapPathIsNull_ReturnServerMapPath()
        {
            // Act
            baseHandler.MapPath = null;

            // Assert
            Assert.Equal(baseHandler.MapPath, HostingEnvironment.MapPath);
        }

        [Fact]
        public void WriteLog_Always_CallMonitorServiceWriteLog()
        {
            // Arrange
            var tickets = BetListDataTest.Tickets;
            var custId = 1;

            // Act
            baseHandler.WriteLog(custId, tickets);

            // Assert
            betlistFixture.MonitorService.Received().WriteLog(
                Arg.Is<int>(cid => cid == custId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(tickets)));
        }

        [Fact]
        public void BuildProductSelectListItems_NullProducts_ReturnsEmptySelectListItem()
        {
            // Arrange
            IEnumerable<Product> products = null;
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(Enumerable.Empty<int>(), products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_EmptyProductList_ReturnsEmptySelectListItem()
        {
            // Arrange
            var products = Enumerable.Empty<Product>();
            var userSelectedProductIds = Enumerable.Empty<int>();
            var expectedSelectListItems = Enumerable.Empty<SelectListItem>();

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(userSelectedProductIds, products);

            // Assert
            Assert.Equal(expectedSelectListItems, actualSelectListItems);
        }

        [Fact]
        public void BuildProductSelectListItems_HasDataProductList_ReturnsCorrectSelectListItem()
        {
            // Arrange
            Func<int, string> getProductDisplayName = (id) => $"Product {id}";

            IEnumerable<Product> products = new List<Product>
            {
                new Product(1, "Product 1", null, getProductDisplayName),
                new Product(2, "Product 2", null, getProductDisplayName),
                new Product(3, "Product 3", null, getProductDisplayName)
            };

            var userSelectedProductIds = new List<int> { 1, 3 };
            IEnumerable<SelectListItem> expectedSelectListItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Product 1",
                    Value = "1",
                    Selected = true
                },
                new SelectListItem
                {
                    Text = "Product 2",
                    Value = "2",
                    Selected = false
                },
                new SelectListItem
                {
                    Text = "Product 3",
                    Value = "3",
                    Selected = true
                }
            };

            // Act
            var actualSelectListItems = baseHandler.BuildProductSelectListItems(userSelectedProductIds, products).ToList();

            // Assert
            Assert.True(compareLogic.Compare(expectedSelectListItems, actualSelectListItems).AreEqual);
        }
    }
}