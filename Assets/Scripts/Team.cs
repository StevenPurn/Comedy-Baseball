using System.Xml.Serialization;
using System.Collections.Generic;

[XmlType("team")]
public class Team
{
    public static string xmlRoot = "teams";

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

    public List<Player> players = new List<Player>();
    public int score = 0;
}