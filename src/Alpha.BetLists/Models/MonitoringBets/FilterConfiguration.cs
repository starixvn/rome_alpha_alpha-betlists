namespace Alpha.BetLists.Models.MonitoringBets
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public class FilterConfiguration
    {
        private FilterXml filterXml;

        public FilterConfiguration()
        {
        }

        public FilterConfiguration(
            int filterId,
            string filterName,
            string filterValue,
            FilterXml filterXml,
            bool isDefault,
            bool isNotify,
            DateTime createdDate,
            string watchList,
            string inputList)
        {
            FilterId = filterId;
            FilterName = filterName;
            FilterValue = filterValue;
            this.filterXml = filterXml;
            IsDefault = isDefault;
            IsNotify = isNotify;
            CreatedDate = createdDate;
            WatchList = watchList;
            InputList = inputList;
        }

        public int FilterId { get; set; }

        public string FilterName { get; set; }

        public string FilterValue { get; set; }

        public FilterXml FilterXml
        {
            get
            {
                filterXml = filterXml ?? Deserialize(FilterValue);

                return filterXml;
            }
        }

        public bool IsDefault { get; set; }

        public bool IsNotify { get; set; }

        public DateTime CreatedDate { get; set; }

        public string WatchList { get; set; }

        public string InputList { get; set; }

        private static FilterXml Deserialize(string xml)
        {
            var serializer = new XmlSerializer(typeof(FilterXml));

            using (TextReader reader = new StringReader(xml))
            {
                return (FilterXml)serializer.Deserialize(reader);
            }
        }
    }
}
