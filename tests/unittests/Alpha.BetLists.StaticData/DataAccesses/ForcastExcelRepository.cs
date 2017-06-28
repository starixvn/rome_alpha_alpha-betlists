namespace Alpha.BetLists.StaticData.DataAccesses
{
    using IntegrationTest.Npoi;
    using Models;
    using NPOI.SS.UserModel;

    public class ForcastExcelRepository : BetListExcelRepository
    {
        public override T Create<T>(IRow row)
        {
            var columnIndex = 0;

            object result = new ScoreMap(
                typeId: row.GetCellValue<int>(columnIndex++),
                eventType: row.GetCellValue<int>(columnIndex++),
                firstId: row.GetCellValue<string>(columnIndex++),
                infoId: row.GetCellValue<string>(columnIndex));

            return (T)result;
        }
    }
}
