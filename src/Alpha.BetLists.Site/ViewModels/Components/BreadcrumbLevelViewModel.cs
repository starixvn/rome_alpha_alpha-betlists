namespace Alpha.BetLists.Site.ViewModels.Components
{
    public class BreadcrumbLevelViewModel
    {
        public BreadcrumbLevelViewModel(int custId, string custName, byte custLevelId)
        {
            CustId = custId;
            CustName = custName;
            CustLevelId = custLevelId;
        }

        public int CustId { get; }

        public string CustName { get; }

        public byte CustLevelId { get; }
    }
}