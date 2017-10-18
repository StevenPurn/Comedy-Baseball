using System.Xml.Serialization;

[XmlType("team")]
public class Team
{
    public static string xmlRoot = "ArrayOfTeam";

    [XmlElement("id")]
    public int id { get; set; }
    [XmlElement("name")]
    public string name { get; set; }
    [XmlElement("wins")]
    public int wins { get; set; }
    [XmlElement("loses")]
    public int loses { get; set; }
    [XmlElement("ties")]
    public int ties { get; set; }
    [XmlElement("runs")]
    public int runs { get; set; }
    [XmlElement("hits")]
    public int hits { get; set; }
    [XmlElement("atbatstrikeouts")]
    public int atbatstrikeouts { get; set; }
    [XmlElement("pitchedstrikeouts")]
    public int pitchedstrikeouts { get; set; }
}