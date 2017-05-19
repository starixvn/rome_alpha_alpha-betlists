namespace Alpha.BetLists.DataAccesses
{
    using Fanex.BetListDetail.Lucky3.Criteria;
    using Fanex.BetListDetail.Lucky3.DataAccesses;
    using Fanex.BetListDetail.Lucky3.Models;
    using Helpers;
    using System.Collections.Generic;

    public class Lucky3ExcelDataAccess : ILucky3DataAccess
    {
        public IEnumerable<Lucky3DetailModel> GetLucky3Detail(IGetLucky3DetailParameter criteria)
        {
            var details = NpoiHelper.LoadData<Lucky3DetailModel>(
                "Lucky3",
                row => new Lucky3DetailModel
                {
                    TransactionDescription = row.GetCell(0).StringCellValue,
                    Hits = (int)row.GetCell(1).NumericCellValue,
                    Bets = (int)row.GetCell(2).NumericCellValue,
                    Prize = (int)row.GetCell(3).NumericCellValue,
                    Stake = (decimal)row.GetCell(4).NumericCellValue,
                    Winlost = (decimal)row.GetCell(5).NumericCellValue,
                    SuperDiscount = (decimal)row.GetCell(6).NumericCellValue,
                    SuperCommission = (decimal)row.GetCell(7).NumericCellValue,
                    MasterDiscount = (decimal)row.GetCell(8).NumericCellValue,
                    MasterCommission = (decimal)row.GetCell(9).NumericCellValue,
                    AgentDiscount = (decimal)row.GetCell(10).NumericCellValue,
                    AgentCommission = (decimal)row.GetCell(11).NumericCellValue,                    
                    Status = row.GetCell(12).StringCellValue,
                });
            return details;
        }
    }
}