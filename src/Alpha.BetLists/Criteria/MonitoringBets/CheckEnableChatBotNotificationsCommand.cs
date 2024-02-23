using Alpha.BetLists.Constants;
using Fanex.Data.Repository;

namespace Alpha.BetLists.Criteria.MonitoringBets
{
    public class CheckEnableChatBotNotificationsCommand : NonQueryCommand
    {
        public CheckEnableChatBotNotificationsCommand(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
            FunctionID = ChatBotFunctionIdConstants.LastBets;
        }

        public int CustId { get; }
        public int SubAccId { get; }
        public int FunctionID { get; }
        public bool Status { get; set; }

        public override string GetSettingKey()
            => "Age_Chatbot_StatusCheck";

        public override bool IsValid() => CustId > 0 && SubAccId >= 0;
    }
}
