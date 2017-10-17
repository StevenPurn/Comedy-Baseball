using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [XmlAttribute("name")]
    new public string name;

    public float battingAvg;
}
