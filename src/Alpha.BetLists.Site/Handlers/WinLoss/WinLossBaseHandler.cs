namespace Alpha.BetLists.Site.Handlers.WinLoss
{
    using Extensions;
    using Params;
    using Requests.WinLoss;

    public class WinLossBaseHandler : BetListBaseHandler
    {
        protected WinLossParam CreateWinLossParam(WinLossQuery message)
        {
            return new WinLossParam(
               message.CustId,
               message.ProductIds.ToListInt(),
               message.FromDate,
               message.ToDate,
               message.Language);
        }
    }
}