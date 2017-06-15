namespace Alpha.BetLists.StaticData.Helpers
{
    using IntegrationTest;
    using NPOI.SS.UserModel;
    using System.Web;

    public static class NpoiHelper
    {
        public static IWorkbook LoadWorkbook(string excelFileName)
        {
            var templatePath = HttpContext.Current.Server.MapPath($"~/App_Data/ExcelData/{excelFileName}.xls");
            return NpoiWorkbook.Load(templatePath);
        }
    }
}
