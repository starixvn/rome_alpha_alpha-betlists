namespace Alpha.BetLists.StaticData.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.POIFS.FileSystem;

    public static class NpoiHelper
    {
        public static IEnumerable<T> LoadData<T>(string excelFileName, Func<IRow, T> mapping, int sheetIndex = 0)
        {
            var sheet = LoadSheet(excelFileName, sheetIndex);
            var items = new List<T>();

            foreach (IRow row in sheet)
            {
                if (row.Cells.Any())
                {
                    items.Add(mapping(row));
                }
            }

            return items;
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

        public static IWorkbook LoadWorkbook(string excelFileName)
        {
            var templatePath = HttpContext.Current.Server.MapPath($"~/App_Data/ExcelData/{excelFileName}.xls");
            using (StreamReader input = new StreamReader(templatePath))
            {
                IWorkbook workbook = new HSSFWorkbook(new POIFSFileSystem(input.BaseStream));

                return workbook;
            }
        }

        public static string GetValue(ICell cell, DataFormatter dataFormatter, IFormulaEvaluator formulaEvaluator)
        {
            var result = string.Empty;

            if (cell != null)
            {
                if (cell.CellType == CellType.Numeric ||
                    (cell.CellType == CellType.Formula &&
                    cell.CachedFormulaResultType == CellType.Numeric))
                {
                    result = formulaEvaluator.EvaluateInCell(cell).NumericCellValue.ToString();
                }
                else
                {
                    result = dataFormatter.FormatCellValue(cell, formulaEvaluator);
                }
            }

            return result.Trim();
        }
    }
}