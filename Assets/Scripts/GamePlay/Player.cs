using System.Xml.Serialization;

/// <summary>
///  The players that exist in the XML file.
/// </summary>
[XmlType("player")]
public class Player {

    public static string xmlRoot = "ArrayOfPlayer";

    [XmlElement("name")]
    public string name { get; set; }
    [XmlElement("atbats")]
    public int atBats { get; set; }
    [XmlElement("hits")]
    public int hits { get; set; }
    [XmlElement("battingAvg")]
    public float battingAvg;
    [XmlElement("pitches")]
    public int pitches { get; set; }
    [XmlElement("strikeouts")]
    public int strikeouts { get; set; }
    [XmlElement("portraitPath")]
    public string portraitPath;
}
