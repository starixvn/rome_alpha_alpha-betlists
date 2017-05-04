namespace Alpha.BetLists.Site.Tests.ViewModels
{
    using BetLists.Tests.TestData;
    using Site.ViewModels;
    using Xunit;

    [Trait("Category", "BetListBase")]
    public class ExcelFileViewModelTests
    {
        [Theory]
        [InlineData("", "no-name.xls")]
        [InlineData(" ", "no-name.xls")]
        [InlineData(null, "no-name.xls")]
        [InlineData("/////excel", "excel.xls")]
        [InlineData("excel", "excel.xls")]
        public void Name_Always_SetInCtor(string name, string expectedName)
        {
            // Act
            var excelViewModel = new ExcelFileViewModel(name, BetListDataTest.ExcelFileContent);

            // Assert
            Assert.Equal(expectedName, excelViewModel.Name);
        }

        [Fact]
        public void Content_Always_SetInCtor()
        {
            // Act
            var excelViewModel = new ExcelFileViewModel("excel", BetListDataTest.ExcelFileContent);

            // Assert
            Assert.Equal(BetListDataTest.ExcelFileContent, excelViewModel.Content);
        }

        [Fact]
        public void ContentType_Always_GetDefaultValue()
        {
            // Act
            var excelViewModel = new ExcelFileViewModel("excel", BetListDataTest.ExcelFileContent);

            // Assert
            Assert.Equal("application/vnd.ms-excel", excelViewModel.ContentType);
        }
    }
}