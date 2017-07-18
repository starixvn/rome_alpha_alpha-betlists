namespace Alpha.BetLists.Models.CancelledBet
{
    public class CancelledBetModel
    {
        public CancelledBetModel(FilterCancelledBetModel filter, BetListData betListData)
        {
            Filter = filter;
            BetListData = betListData;
        }

        public FilterCancelledBetModel Filter { get; }

        public BetListData BetListData { get; }
    }
}
