using System.Xml.Serialization;

namespace shimmer.DTO
{
    [XmlRoot(ElementName = "timestamp")]
    public class NistTime
    {
        [XmlAttribute(AttributeName = "time")]
        public string Time { get; set; }
        [XmlAttribute(AttributeName = "delay")]
        public string Delay { get; set; }
    }

}
