namespace Alpha.BetLists.Models
{
    using Alpha.BetLists.AppResources;

    public class UsernameErrCode
    {
        public UsernameErrCode()
        {
        }

        public UsernameErrCode(
            int custId,
            string username,
            string username2,
            string firstName,
            string lastName,
            int errCode)
        {
            CustId = custId;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Username2 = username2;
            ErrCode = errCode;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Username2 { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public int ErrCode { get; }

        public string ErrMsg => ErrCode == 0 ? string.Empty : Resources.NotAMember;
    }
}
