using System.Xml.Serialization;

/// <summary>
///  The players that exist in the XML file.
/// </summary>
[XmlType("player")]
public class Player {

    public static string xmlRoot = "ArrayOfPlayer";

    [XmlElement("number")]
    public int number;
    [XmlElement("name")]
    public string name { get; set; }
    [XmlElement("atbats")]
    public int atBats { get; set; }
    [XmlElement("hits")]
    public int hits { get; set; }
    [XmlElement("battingAvg")]
    public float battingAvg;
    [XmlElement("rbi")]
    public int rbis;
    [XmlElement("strikeoutsAtBat")]
    public int strikeoutsAtBat { get; set; }
    [XmlElement("strikeoutsPitched")]
    public int strikeoutsPitched { get; set; }
    [XmlElement("portraitPath")]
    public string portraitPath;
    [XmlElement("team")]
    public string team;
}
