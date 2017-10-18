using UnityEngine.UI;
using UnityEngine;
using System;

public class GetTeamsAndPlayers : MonoBehaviour {

    public Text text;

	//Eventually use this to set up the pregame screen
	void Start () {
        text = GetComponent<Text>();

        string teamsAndPlayers;

        ActiveTeam team1 = GameControl.instance.activeTeams[0];
        ActiveTeam team2 = GameControl.instance.activeTeams[1];
        teamsAndPlayers = String.Format("Team 1: {0}\nFeaturing:\n{1}\n{2}\n{3}\n", team1.name, team1.players[0].name, team1.players[1].name, team1.players[2].name) 
            + String.Format("Team 2: {0}\nFeaturing:\n{1}\n{2}\n{3}\n", team2.name, team2.players[0].name, team2.players[1].name, team2.players[2].name);
        teamsAndPlayers += "\nPress 'Space' to continue";
        text.text = teamsAndPlayers;
	}
}