using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayers : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (var team in GameControl.instance.activeTeams)
        {
            Debug.Log("Team name: " + team.name);
            foreach (var player in team.players)
            {
                Debug.Log("Player: " + player.name);
            }
        }
	}
}
