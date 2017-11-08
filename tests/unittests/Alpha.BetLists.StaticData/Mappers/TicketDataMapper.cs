namespace Alpha.BetLists.StaticData.Mappers
{
    using Alpha.StaticData.Npoi;
    using Fanex.BetList.Core.Entities;
    using NPOI.SS.UserModel;
    using static ColumnIndexes.TicketDataTemplateColumnIndex;

    public static class TicketDataMapper
    {
        public static ITicketData Map(IRow row)
            => new TicketData
            {
                AgentPT = row.Get<decimal>(AgentPT),
                BetTeam = row.Get<string>(BetTeam),
                BetTypeId = row.Get<int>(BetTypeId),
                MasterPT = row.Get<decimal>(MasterPT),
                Odds = row.Get<decimal>(Odds),
                RefNo = row.Get<string>(RefNo),
                Stake = row.Get<decimal>(Stake),
                Status = row.Get<string>(Status),
                SuperPT = row.Get<decimal>(SuperPT),
                TransDesc = row.Get<string>(TransDesc)
            };
    }
}
