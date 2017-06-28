namespace Alpha.BetLists.StaticData.DataAccesses
{
    using IntegrationTest.Npoi;
    using Models;
    using NPOI.SS.UserModel;

    public class ProductExcelRepository : BetListExcelRepository
    {
        public override T Create<T>(IRow row)
        {
            var columnIndex = 0;

            object result = new Product(
                id: row.GetCellValue<int>(columnIndex++),
                name: row.GetCellValue<string>(columnIndex));

            return (T)result;
        }
    }
}
