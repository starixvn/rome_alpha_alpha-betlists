namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Helpers;
    using Models;
    using System.Collections.Generic;

    public static class ProductExcelRepository
    {
        public static IEnumerable<Product> GetProducts()
        {
            return NpoiHelper.LoadData(
                "Products",
                row => new Product(
                        (int)row.GetCell(0).NumericCellValue,
                        row.GetCell(1).StringCellValue));
        }
    }
}