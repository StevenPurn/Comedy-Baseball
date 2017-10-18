using System.Xml;
using System.Xml.Serialization;

[XmlType("player")]
public class Player {

    public static string xmlRoot = "players";

    [XmlElement("name")]
    public string name { get; set; }
    [XmlElement("atbats")]
    public int atBats { get; set; }
    [XmlElement("hits")]
    public int hits { get; set; }
    public float battingAvg;
}
