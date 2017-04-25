namespace Alpha.BetLists.Site.Tests.Handler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using BetLists.Tests.TestData;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fixtures;
    using Handlers;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "BaseBetList")]
    [Collection("BetListCollection")]
    public class BetListBaseHandlerTests
    {
        private readonly BetListFixture betlistFixture;
        private readonly BetListBaseHandler baseHandler;

        public BetListBaseHandlerTests(BetListFixture betlistFixture)
        {
            this.betlistFixture = betlistFixture;

            baseHandler = new BetListBaseHandler(
                this.betlistFixture.HtmlBuilder,
                this.betlistFixture.ExcelBuilder,
                this.betlistFixture.TicketHelper,
                this.betlistFixture.MoniterService,
                this.betlistFixture.IpService);
        }

        [Fact]
        public void Builder_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.Builder, betlistFixture.HtmlBuilder);
        }

        [Fact]
        public void ExcelBuilder_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.ExcelBuilder, betlistFixture.ExcelBuilder);
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
            Assert.Equal(baseHandler.MonitorService, betlistFixture.MoniterService);
        }

        [Fact]
        public void IpService_Always_SetValueFromCtor()
        {
            // Act & Assert
            Assert.Equal(baseHandler.IpService, betlistFixture.IpService);
        }

        [Fact]
        public void RtfHelper_Always_SetValueFromExcelBuilder()
        {
            // Act & Assert
            Assert.Equal(baseHandler.RtfHelper, betlistFixture.ExcelBuilder.RTFHelper);
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void GetExcelTemplatePath_TemplateIsNullOrWhiteSpace_ThrowArgumentNullException(string fileName)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => baseHandler.GetExcelTemplatePath(fileName));
        }

        [Fact]
        public void GetExcelTemplatePath_FileDoesNotExist_ThrowFileNotFoundException()
        {
            // Arrange
            baseHandler.MapPath = path => "path";
            baseHandler.IsFileExist = path => false;

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => baseHandler.GetExcelTemplatePath("test"));
        }

        [Fact]
        public void GetExcelTemplatePath_FileExists_ReturnPath()
        {
            // Arrange
            baseHandler.MapPath = relativePath => "path/" + relativePath;
            baseHandler.IsFileExist = absolutePath => true;

            // Act
            var path = baseHandler.GetExcelTemplatePath("test");

            // Assert
            Assert.Equal("path/~/App_Data/ExcelTemplates/test.xls", path);
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
            betlistFixture.MoniterService.Received().WriteLog(
                Arg.Is<int>(cid => cid == custId),
                Arg.Is<IList<ITicket>>(t => t.SequenceEqual(tickets)));
        }
    }
}