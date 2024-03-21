using System.Xml.Serialization;

namespace shimmer.DTO
{
    /// <summary>
    /// Store the returned xml data from url
    /// </summary>
    [XmlRoot(ElementName = "timestamp")]
    public class NistTime
    {
        [XmlAttribute(AttributeName = "time")]
        public string Time { get; set; }
        [XmlAttribute(AttributeName = "delay")]
        public string Delay { get; set; }
    }

}
