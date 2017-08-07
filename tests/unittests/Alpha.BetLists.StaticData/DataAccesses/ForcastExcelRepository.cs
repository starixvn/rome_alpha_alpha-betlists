namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Alpha.StaticData.Npoi;
    using BetLists.Models;
    using NPOI.SS.UserModel;

    public class ForcastExcelRepository : BetListExcelRepository
    {
        public override T Create<T>(IRow row)
        {
            var columnIndex = 0;

            object result = new ScoreMap(
                typeId: row.Get<int>(columnIndex++),
                eventType: row.Get<int>(columnIndex++),
                firstId: row.Get<string>(columnIndex++),
                infoId: row.Get<string>(columnIndex));

            return (T)result;
        }
    }
}
