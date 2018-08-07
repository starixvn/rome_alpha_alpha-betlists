namespace Alpha.BetLists.Models
{
    using System.Collections.Generic;

    public class PagedReportData<T> where T : class
    {
        public PagedReportData(
            IEnumerable<T> dataSource,
            int pageIndex,
            int pageSize,
            int totalRecords)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            DataSource = dataSource;
        }

        public IEnumerable<T> DataSource { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalRecords { get; }

        public bool Any() => TotalRecords > 0;

        public bool MoreThanOne() => TotalRecords > 1;
    }
}
