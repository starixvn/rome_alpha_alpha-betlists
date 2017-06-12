namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Helpers;
    using Models;
    using System.Collections.Generic;

    public class ForcastExcelRepository
    {
        public static IEnumerable<ScoreMap> GetScoreMaps()
        {
            return NpoiHelper.LoadData(
                "ScoreMaps",
                row => new ScoreMap(
                        (int)row.GetCell(0).NumericCellValue,
                        (int)row.GetCell(1).NumericCellValue,
                        row.GetCell(2).StringCellValue,
                        row.GetCell(3).StringCellValue));
        }
    }
}