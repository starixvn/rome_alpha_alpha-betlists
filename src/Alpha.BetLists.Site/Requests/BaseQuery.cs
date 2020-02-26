namespace Alpha.BetLists.Site.Requests
{
    using System;
    using BetLists.Models;

    public abstract class BaseQuery
    {
        protected BaseQuery(IUser viewer)
        {
            // Default data, if query string had data, then get from query string.
            CustId = viewer.Id;
            CustName = viewer.Name;

            ViewerId = viewer.Id;
            ViewerLevelId = viewer.LevelId;
            ViewerLevel = Enumeration.FromValue<UserLevel>(ViewerLevelId);
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            Language = viewer.Language;
            IsEnabledJackpot = viewer.IsEnabledJackpot;

            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }

        public string Language { get; set; }

        public int ViewerId { get; }

        public byte ViewerLevelId { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public bool DisplayVNIP { get; }

        public UserLevel ViewerLevel { get; }

        public string Type { get; set; }

        public bool IsHistoricReport { get; set; }

        public bool IsEnabledJackpot { get; set; }

        public DateTime LimitedDateToShowInfo { get; set; }

        public bool ViewReport { get; }
    }
}