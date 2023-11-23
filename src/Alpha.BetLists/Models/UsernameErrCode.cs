namespace Alpha.BetLists.Models
{
    using Alpha.BetLists.AppResources;

    public class UsernameErrCode
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Username2 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg => ErrCode == 0 ? string.Empty : Resources.NotAMember;
    }
}
