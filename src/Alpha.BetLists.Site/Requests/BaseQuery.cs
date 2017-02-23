namespace Alpha.BetLists.Site.Requests
{
    using System;
    using BetLists.Models;

    public abstract class BaseQuery
    {
        protected BaseQuery(IUser viewer)
        {
            ViewerId = viewer.Id;
            ViewerLevelId = viewer.LevelId;
            ViewerLevel = Enumeration.FromValue<UserLevel>(ViewerLevelId);
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductIds { get; set; }

        public string Language { get; set; }

        public int ViewerId { get; }

        public byte ViewerLevelId { get; }

        public UserLevel ViewerLevel { get; }
    }
}