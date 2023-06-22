namespace Alpha.BetLists.Models.MonitoringBets
{
    using System.Xml.Serialization;

    public class SportXml
    {
        [XmlAttribute(AttributeName = "s")]
        public string Sport { get; set; }

        [XmlAttribute(AttributeName = "l")]
        public string League { get; set; }

        [XmlAttribute(AttributeName = "m")]
        public int Match { get; set; }

        [XmlAttribute(AttributeName = "h")]
        public int Home { get; set; }

        [XmlAttribute(AttributeName = "a")]
        public int Away { get; set; }

        [XmlAttribute(AttributeName = "iN")]
        public bool IsNeutral { get; set; }
    }
}
