namespace Alpha.BetLists.Builders.PTComm
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Models;

    public class PTComm2198 : PTComm
    {
        public PTComm2198(ITicket ticket, UserLevel userLevel)
            : base(ticket, userLevel)
        {
        }

        public override string PositionTaking => FormatPositionTaking(PtByUserLevel[UserLevel]);

        private static string FormatPositionTaking(object value)
        {
            var ptValue = Cast.AsDecimal(value);

            return ptValue < 0
                ? string.Format(ConvertByBetType.Minus, string.Format(Formatter.DECIMALFORMAT40, ptValue))
                : string.Format(Formatter.DECIMALFORMAT40, ptValue);
        }
    }
}