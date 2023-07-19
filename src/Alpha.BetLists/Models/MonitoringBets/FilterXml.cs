namespace Alpha.BetLists.Models.MonitoringBets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Filter")]
    public class FilterXml
    {
        [XmlAttribute(AttributeName = "b")]
        public string BetType { get; set; }

        [XmlAttribute(AttributeName = "st")]
        public int Stake { get; set; }

        [XmlAttribute(AttributeName = "members")]
        public string Members { get; set; }

        [XmlAttribute(AttributeName = "wl")]
        public string MyWatchlistMembers { get; set; }

        [XmlAttribute(AttributeName = "iWL")]
        public byte IsWatchlist { get; set; }

        [XmlAttribute(AttributeName = "iL")]
        public byte IsLive { get; set; }

        [XmlElement(ElementName = "Sport")]
        public List<SportXml> Sports { get; set; }

        [XmlIgnore]
        public IEnumerable<string> BetTypeNames { get; set; }

        [XmlIgnore]
        public IEnumerable<Sport> FlatternSports { get; set; }

        [XmlIgnore]
        public bool IsAllSports => !FlatternSports.Any();
    }
}
