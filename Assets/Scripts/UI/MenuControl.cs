using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

    [Header("Team 1")]
    public Dropdown team1;
    public Dropdown t1player1, t1player2, t1player3;

    [Header("Team 2")]
    public Dropdown team2;
    public Dropdown t2player1, t2player2, t2player3;

    public void AdvanceToNextScene(string nextScene)
    {
        SetupTeams();
        if (CheckForIssues())
        {
            return;
        }
        SceneManager.LoadScene(nextScene);
    }

    bool CheckForIssues()
    {
        bool result = false;

        Debug.Log("check for any issues with this screen");

        return result;
    }

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
}
