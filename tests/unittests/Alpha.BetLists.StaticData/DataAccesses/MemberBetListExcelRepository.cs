namespace Alpha.BetLists.StaticData.DataAccesses
{
    using Fanex.BetList.Core.Entities;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class MemberBetListExcelRepository
    {
        public static async Task<Tuple<IEnumerable<TicketDto>, IEnumerable<ITicketData>>> GetMemberBetList()
        {
            var excelDataAccess = new TicketExcelDataAccess("MemberBetlist");

            var result = new Tuple<IEnumerable<TicketDto>, IEnumerable<ITicketData>>(excelDataAccess.GetTickets(), excelDataAccess.GetTicketData());

            return await Task.FromResult(result);
        }
    }
}