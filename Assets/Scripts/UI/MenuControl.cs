using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour {

    [Header("Team 1")]
    public Dropdown team1;
    public Dropdown t1player1, t1player2, t1player3;
    public Toggle t1Toggle;

    [Header("Team 2")]
    public Dropdown team2;
    public Dropdown t2player1, t2player2, t2player3;
    public Toggle t2Toggle;

    [Header("Error Text")]
    public GameObject errorObj;
    public Text errorText;
    public string error;

    [Header("Other")]
    public Toggle ballHitToggle;
    public GameObject confirmationDialog;

    //Go to the next scene unless there are invalid selections
    public void AdvanceToNextScene(string nextScene)
    {
        if (CheckForIssues())
        {
            return;
        }
        SetupTeams();
        GameControl.playHitAudio = ballHitToggle.isOn;
        SceneManager.LoadScene(nextScene);
    }

    public void OnTeamChanged()
    {
        foreach (var team in GameControl.instance.teams)
        {
            if (team.name == team1.options[team1.value].text)
            {
                Color teamColor = new Color(team.colorR, team.colorG, team.colorB);
                GameControl.instance.homeTeamMat.color = teamColor;
                var count = 0;
                foreach (var player in GameControl.instance.players)
                {
                    if (team.name == player.team)
                    {
                        if (count < 1)
                        {
                            count++;
                            for (int i = 0; i < t1player1.options.Count; i++)
                            {
                                if(t1player1.options[i].text == player.name)
                                {
                                    t1player1.value = i;
                                }
                            }
                        }
                        else if (count == 1)
                        {
                            count++;
                            for (int i = 0; i < t1player1.options.Count; i++)
                            {
                                if (t1player2.options[i].text == player.name)
                                {
                                    t1player2.value = i;
                                }
                            }
                        }
                        else if (count > 1)
                        {
                            for (int i = 0; i < t1player1.options.Count; i++)
                            {
                                if (t1player3.options[i].text == player.name)
                                {
                                    t1player3.value = i;
                                }
                            }
                        }
                    }
                }
            }
            if (team.name == team2.options[team2.value].text)
            {
                Color teamColor = new Color(team.colorR, team.colorG, team.colorB);
                GameControl.instance.awayTeamMat.color = teamColor;
                var count = 0;
                foreach (var player in GameControl.instance.players)
                {
                    if (team.name == player.team)
                    {
                        if (count < 1)
                        {
                            count++;
                            for (int i = 0; i < t2player1.options.Count; i++)
                            {
                                if (t2player1.options[i].text == player.name)
                                {
                                    t2player1.value = i;
                                }
                            }
                        }
                        else if (count == 1)
                        {
                            count++;
                            for (int i = 0; i < t2player2.options.Count; i++)
                            {
                                if (t2player2.options[i].text == player.name)
                                {
                                    t2player2.value = i;
                                }
                            }
                        }
                        else if (count > 1)
                        {
                            for (int i = 0; i < t2player3.options.Count; i++)
                            {
                                if (t2player3.options[i].text == player.name)
                                {
                                    t2player3.value = i;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //Prevent players from being chosen twice
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
                aTeam.abbreviation = team.abbreviation;
                aTeam.currentlyAtBat = t1Toggle.isOn;
                aTeam.score = 0;
                aTeam.wins = team.wins;
                aTeam.ties = team.ties;
                aTeam.loses = team.loses;
                AddPlayers(aTeam, 1);
                GameControl.instance.activeTeams.Add(aTeam);
                Color teamColor = new Color(team.colorR, team.colorG, team.colorB);
                GameControl.instance.homeTeamMat.color = teamColor;
                aTeam.color = teamColor;
                break;
            }
        }

        foreach (var team in GameControl.instance.teams)
        {
            if (team.name == team2.options[team2.value].text)
            {
                ActiveTeam aTeam = new ActiveTeam();
                aTeam.name = team.name;
                aTeam.abbreviation = team.abbreviation;
                aTeam.score = 0;
                aTeam.currentlyAtBat = t2Toggle.isOn;
                aTeam.wins = team.wins;
                aTeam.ties = team.ties;
                aTeam.loses = team.loses;
                AddPlayers(aTeam, 2);
                GameControl.instance.activeTeams.Add(aTeam);
                Color teamColor = new Color(team.colorR, team.colorG, team.colorB);
                GameControl.instance.awayTeamMat.color = teamColor;
                aTeam.color = teamColor;
                break;
            }
        }
    }

    public void ClearSavedData()
    {
        foreach (var player in GameControl.instance.players)
        {
            ClearPlayerData(player);
        }

        foreach (var team in GameControl.instance.teams)
        {
            ClearTeamData(team);
        }

        SaveLoad.SaveData(GameControl.instance.teamFilePath, GameControl.instance.teams);
        SaveLoad.SaveData(GameControl.instance.playerFilePath, GameControl.instance.players);
    }

    private void ClearPlayerData(Player player)
    {
        player.atBats = 0;
        player.hits = 0;
        player.rbis = 0;
        player.strikeoutsAtBat = 0;
        player.strikeoutsPitched = 0;
        player.battingAvg = 0;
    }

    private void ClearTeamData(Team team)
    {
        team.wins = 0;
        team.loses = 0;
        team.ties = 0;
        team.runs = 0;
        team.pitchedstrikeouts = 0;
        team.hits = 0;
        team.atbatstrikeouts = 0;
    }

    public void OpenConfirmationDialog(bool isActive)
    {
        confirmationDialog.SetActive(isActive);
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
                        aPlayer.number = player.number;
                        aPlayer.name = player.name;
                        aPlayer.totalAtBats = player.atBats;
                        aPlayer.totalHits = player.hits;
                        aPlayer.portraitPath = player.portraitPath;
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
                        aPlayer.number = player.number;
                        aPlayer.name = player.name;
                        aPlayer.totalAtBats = player.atBats;
                        aPlayer.totalHits = player.hits;
                        aPlayer.portraitPath = player.portraitPath;
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
