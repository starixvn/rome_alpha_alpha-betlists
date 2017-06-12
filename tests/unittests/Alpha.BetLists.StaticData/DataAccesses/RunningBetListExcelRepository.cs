namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Fanex.BetList.Core.Entities;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RunningBetListExcelRepository
    {
        public static async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<ITicketData>>> GetRunningBetList()
        {
            var excelDataAccess = new TicketExcelDataAccess("RunningBetlist");

            var result = new Tuple<IEnumerable<TicketDto>, IEnumerable<ITicketData>>(excelDataAccess.GetTickets(), excelDataAccess.GetTicketData());

            return await Task.FromResult(result);
        }
    }
}