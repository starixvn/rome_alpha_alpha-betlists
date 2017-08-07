namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Alpha.StaticData.Npoi;
    using BetLists.Models;
    using NPOI.SS.UserModel;

    public class ProductExcelRepository : BetListExcelRepository
    {
        public override T Create<T>(IRow row)
        {
            var columnIndex = 0;

            object result = new Product(
                id: row.Get<int>(columnIndex++),
                name: row.Get<string>(columnIndex));

            return (T)result;
        }
    }
}
