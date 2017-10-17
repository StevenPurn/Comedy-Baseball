using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {

    public List<Player> players = new List<Player>();
    new public string name;
    public int score;
    public Inning[] innings = new Inning[GameControl.numberOfInnings];
    
}
