namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Helpers;
    using IntegrationTest;
    using Models;
    using System.Collections.Generic;

    public static class ProductExcelRepository
    {
        public static IEnumerable<Product> GetProducts()
        {
            var workbook = NpoiHelper.LoadWorkbook("Products");
            var sheet = workbook?.LoadSheet();

            return sheet?.LoadData(row => new Product(
                  (int)row.GetCell(0).NumericCellValue,
                  row.GetCell(1).StringCellValue));
        }
    }
}
