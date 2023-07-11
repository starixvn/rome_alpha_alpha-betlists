namespace Alpha.BetLists.Models.MonitoringBets
{
    using System.Xml.Serialization;

    public class SportXml
    {
        [XmlAttribute(AttributeName = "s")]
        public string Sport { get; set; }

        [XmlAttribute(AttributeName = "l")]
        public string League { get; set; }
    }
}
