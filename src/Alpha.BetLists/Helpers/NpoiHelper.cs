using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Alpha.BetLists.Helpers
{
    public static class NpoiHelper
    {
        public static IEnumerable<T> LoadData<T>(string excelFileName, Func<IRow, T> mapping, int sheetIndex = 0)
        {
            var sheet = LoadSheet(excelFileName, sheetIndex);

            foreach (IRow row in sheet)
            {
                if (row.Cells.Any())
                {
                    yield return mapping(row);
                }
            }
        }

        public static ISheet LoadSheet(string excelFileName, int sheetIndex = 0)
        {
            var templatePath = HttpContext.Current.Server.MapPath($"~/App_Data/ExcelData/{excelFileName}.xls");
            using (FileStream file = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                var wookbook = new HSSFWorkbook(file);

                var sheet = wookbook.GetSheetAt(sheetIndex);
                sheet.RemoveRow(sheet.GetRow(0));

                return sheet;
            }
        }
    }
}
