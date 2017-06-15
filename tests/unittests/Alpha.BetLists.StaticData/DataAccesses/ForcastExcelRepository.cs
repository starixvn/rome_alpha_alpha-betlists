namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Helpers;
    using IntegrationTest;
    using Models;
    using System.Collections.Generic;

    public static class ForcastExcelRepository
    {
        public static IEnumerable<ScoreMap> GetScoreMaps()
        {
            var workbook = NpoiHelper.LoadWorkbook("ScoreMaps");
            var sheet = workbook?.LoadSheet();

            return sheet?.LoadData(row => new ScoreMap(
                        (int)row.GetCell(0).NumericCellValue,
                        (int)row.GetCell(1).NumericCellValue,
                        row.GetCell(2).StringCellValue,
                        row.GetCell(3).StringCellValue));
        }
    }
}
