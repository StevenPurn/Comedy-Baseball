using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MenuControl : MonoBehaviour {

    [Header("Team 1")]
    public Dropdown team1;
    public Dropdown t1player1, t1player2, t1player3;

    [Header("Team 2")]
    public Dropdown team2;
    public Dropdown t2player1, t2player2, t2player3;

    [Header("Error Text")]
    public GameObject errorObj;
    public Text errorText;
    public string error;

    public void AdvanceToNextScene(string nextScene)
    {
        SetupTeams();
        if (CheckForIssues())
        {
            return;
        }
        SceneManager.LoadScene(nextScene);
    }
    //Prevent players from being listed on a team multiple times
    //or the same team being selected in both options
    bool CheckForIssues()
    {
        bool result = false;

        if(team1.options[team1.value].text == team2.options[team2.value].text)
        {
            error = "Teams can't be the same";
            result = true;
        }

        bool valueDuplicated = CheckValueDuplicated();

        //check if any of the three players match any of the other three players
        if (valueDuplicated)
        {
            result = true;
            if(error.Length > 0)
            {
                error += " & Players can only be chosen once";
            }
            else
            {
                error += "Players can only be chosen once";
            }
        }

        if(result == true)
        {
            errorText.text = error;
            errorObj.SetActive(true);
            error = "";
        }

        return result;
    }

    //Set selected teams as active
    void SetupTeams()
    {
        foreach (var team in GameControl.instance.teams)
        {
            if (team.name == team1.options[team1.value].text)
            {
                ActiveTeam aTeam = new ActiveTeam();
                aTeam.name = team.name;
                AddPlayers(aTeam, 1);
                GameControl.instance.activeTeams.Add(aTeam);
                return;
            }
        }

        foreach (var team in GameControl.instance.teams)
        {
            if (team.name == team2.options[team2.value].text)
            {
                ActiveTeam aTeam = new ActiveTeam();
                aTeam.name = team.name;
                AddPlayers(aTeam, 2);
                GameControl.instance.activeTeams.Add(aTeam);
                return;
            }
        }
    }

    //Add selected players to active team
    void AddPlayers(ActiveTeam team, int teamNumber)
    {
        switch (teamNumber)
        {
            case 1:
                foreach (var player in GameControl.instance.players)
                {
                    if (player.name == t1player1.options[t1player1.value].text || player.name == t1player2.options[t1player2.value].text || player.name == t1player3.options[t1player3.value].text)
                    {
                        ActivePlayer aPlayer = new ActivePlayer();
                        aPlayer.name = player.name;
                        team.players.Add(aPlayer);
                    }
                }
                break;
            case 2:
                foreach (var player in GameControl.instance.players)
                {
                    if (player.name == t2player1.options[t2player1.value].text || player.name == t2player2.options[t2player2.value].text || player.name == t2player3.options[t2player3.value].text)
                    {
                        ActivePlayer aPlayer = new ActivePlayer();
                        aPlayer.name = player.name;
                        team.players.Add(aPlayer);
                    }
                }
                break;
        }
    }

    //Prevent advancing if players have been duplicated
    bool CheckValueDuplicated()
    {
        if(t1player1.options[t1player1.value].text == t2player1.options[t2player1.value].text || t1player1.options[t1player1.value].text == t2player2.options[t2player2.value].text || t1player1.options[t1player1.value].text == t2player3.options[t2player3.value].text)
        {
            return true;
        }else if (t1player2.options[t1player2.value].text == t2player1.options[t2player1.value].text || t1player2.options[t1player2.value].text == t2player2.options[t2player2.value].text || t1player2.options[t1player2.value].text == t2player3.options[t2player3.value].text)
        {
            return true;
        }else if (t1player3.options[t1player3.value].text == t2player1.options[t2player1.value].text || t1player3.options[t1player3.value].text == t2player2.options[t2player2.value].text || t1player3.options[t1player3.value].text == t2player3.options[t2player3.value].text)
        {
            return true;
        }
        else if (t1player3.options[t1player3.value].text == t1player1.options[t1player1.value].text || t1player3.options[t1player3.value].text == t1player2.options[t1player2.value].text)
        {
            return true;
        }
        else if (t2player3.options[t2player3.value].text == t2player1.options[t2player1.value].text || t2player3.options[t2player3.value].text == t2player2.options[t2player2.value].text)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
