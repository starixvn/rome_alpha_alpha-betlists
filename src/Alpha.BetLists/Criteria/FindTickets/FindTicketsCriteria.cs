namespace Alpha.BetLists.Criteria.FindTickets
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;

    public class FindTicketsCriteria : CriteriaBase
    {
        public FindTicketsCriteria(
             long refNo)
        {
            RefNo = refNo;
        }

        public long RefNo{ get; }

        public override string GetSettingKey() => "Age_BetList_SelWinlossByBettypeMatch";

        public override bool IsValid() => RefNo > 0;
    }
}
