namespace Alpha.BetLists.Models.MonitoringBets
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class SportXml
    {
        [XmlAttribute(AttributeName = "s")]
        public string Sport { get; set; }

        [XmlAttribute(AttributeName = "l")]
        public string League { get; set; }
    }
}
