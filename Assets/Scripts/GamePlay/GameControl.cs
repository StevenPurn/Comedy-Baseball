using System;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    //Fake singleton implementation
    public static GameControl instance;
    //Static variables
    public static int numberOfInnings = 3;
    public static Inning curInning;
    public static int strikes, balls, outs;
    //Save location for team & player files
    public string teamFilePath = "/Data/Teams.xml";
    public string playerFilePath = "/Data/Players.xml";
    public GameObject runnerPrefab;
    public Transform battersBox;
    //Teams & players that exist in the xml files
    public List<Team> teams = new List<Team>();
    public List<Player> players = new List<Player>();
    //Teams/players participating in this game
    public List<ActiveTeam> activeTeams = new List<ActiveTeam>();
    public List<ActivePlayer> activePlayers = new List<ActivePlayer>();
    //List of runners in the game at the moment
    public List<Runner> runners = new List<Runner>();

    public Dictionary<TeamColor, Color> teamColors = new Dictionary<TeamColor, Color>()
    {
        {TeamColor.red, Color.red },
        {TeamColor.blue, Color.blue }
    };

    //Load player & team data from xml files
    void Awake () {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        teamFilePath = Application.dataPath + teamFilePath;
        playerFilePath = Application.dataPath + playerFilePath;

        PopulateTeamList();
        PopulatePlayerList();
        curInning.inningNumber = 1;
    }

#region SaveLoad
    //Save list of teams & players to respective xml files
    void Save()
    {
        foreach (var aTeam in activeTeams)
        {
            foreach (var team in teams)
            {
                if(aTeam.name == team.name)
                {
                    UpdateTeamData(aTeam, team);
                }
            }
        }

        foreach (var aPlayer in activePlayers)
        {
            foreach (var player in players)
            {
                if (aPlayer.name == player.name)
                {
                    UpdatePlayerData(aPlayer, player);
                }
            }
        }

        SaveData(teamFilePath, teams);
        SaveData(playerFilePath, players);
    }

    void UpdateTeamData(ActiveTeam aTeam, Team team)
    {
        //Set values of the team to account for updated values in active team
        throw new NotImplementedException();
    }

    void UpdatePlayerData(ActivePlayer aPlayer, Player player)
    {
        //Set values of player to account for updated values in active player
        throw new NotImplementedException();
    }

    //Save data to xml file
    void SaveData(string filePath, System.Object obj)
    {
        //Add new data to active teams & players
        DataHandler.SaveData(filePath, obj);
    }

    //Load team data
    void PopulateTeamList()
    {
        teams = DataHandler.LoadData<List<Team>>(teamFilePath, Team.xmlRoot);
    }

    //Load player data
    void PopulatePlayerList()
    {
        players = DataHandler.LoadData<List<Player>>(playerFilePath, Player.xmlRoot);
    }
#endregion

    //Add team to list of participating teams
    void AddActiveTeams(ActiveTeam team)    
    {
        activeTeams.Add(team);
        foreach (var aTeam in activeTeams)
        {
            foreach (var player in aTeam.players)
            {
                Debug.Log(player.name);
            }
        }
    }

    void NextBatter()
    {
        int teamAtBat = activeTeams[0].currentlyAtBat ? 0 : 1;
        int curBatter = GetCurrentBatter();
        if(curBatter + 1 == activeTeams[teamAtBat].players.Count)
        {
            curBatter = 0;
        }
        else
        {
            curBatter += 1;
        }

        foreach (var player in activeTeams[teamAtBat].players)
        {
            player.isAtBat = false;
        }

        foreach (var runner in runners)
        {
            runner.atBat = false;
        }

        activeTeams[teamAtBat].players[curBatter].isAtBat = true;
        Debug.Log("Currently batting: " + activeTeams[teamAtBat].players[curBatter].name + " for " + activeTeams[teamAtBat].name);
        GameObject go = Instantiate(runnerPrefab, battersBox.position, Quaternion.identity);
    }

    int GetCurrentBatter()
    {
        int teamAtBat = activeTeams[0].currentlyAtBat ? 0 : 1;

        for (int i = 0; i < activeTeams[teamAtBat].players.Count; i++)
        {
            if (activeTeams[teamAtBat].players[i].isAtBat)
            {
                return i;
            }
        }
        return 0;
    }

    ActivePlayer GetCurrentBattingPlayer()
    {
        int teamAtBat = activeTeams[0].currentlyAtBat ? 0 : 1;

        for (int i = 0; i < activeTeams[teamAtBat].players.Count; i++)
        {
            if (activeTeams[teamAtBat].players[i].isAtBat)
            {
                return activeTeams[teamAtBat].players[i];
            }
        }
        return null;
    }

    void SwitchTeamAtBat()
    {
        activeTeams[0].currentlyAtBat = !activeTeams[0].currentlyAtBat;
        activeTeams[1].currentlyAtBat = !activeTeams[1].currentlyAtBat;

        if(activeTeams[0].currentlyAtBat && activeTeams[1].currentlyAtBat)
        {
            Debug.LogWarning("Both teams are batting");
        }else if(activeTeams[0].currentlyAtBat && activeTeams[1].currentlyAtBat)
        {
            Debug.LogWarning("Neither team is batting");
        }
    }

#region HandleInput

    public void HandleStrike(bool wasFoul = false)
    {
        strikes += 1;
        if (strikes >= 3)
        {
            if (wasFoul)
            {
                strikes = 2;
            }
            else
            {
                HandleOut();
            }
        }
    }

    //Need to add potential for advancing runners on base during an out
    public void HandleOut()
    {
        outs += 1;
        Debug.Log("There are " + outs + " outs");
        ResetCount();
        NextBatter();
        if (outs >= 3)
        {
            Debug.Log("Resetting Inning");
            ResetInning();
        }
    }

    public void HandleBall()
    {
        balls += 1;
        if (balls >= 4)
        {
            balls = 4;
        }

        //Walk batter to 1st base & reset count
    }

    public void FastForward()
    {
        Time.timeScale = 1.2f;
    }

    //Respond to hits based on input from Umpire
    //Should add enum for on base/fly out/ground out
    //Can also switch to next batter or next team if there are 3 outs
    public void HandleHit(int bases)
    {
        switch (bases)
        {
            case 1:
                //Play animation of player running to first
                //Advance other runners if not forces?
                //Debug.Log("Player hit a single");
                break;
            case 2:
                //Debug.Log("Player hit a double");
                break;
            case 3:
                //Debug.Log("Player hit a triple");
                break;
            case 4:
                //Debug.Log("Player hit a homerun");
                break;
            default:
                //Debug.Log("Invalid hit");
                break;
        }
        NextBatter();
        ResetCount();
    }

    #endregion

    void ResetCount()
    {
        Debug.Log("Count reset");
        balls = 0;
        strikes = 0;
    } 

    void ResetInning()
    {
        if (curInning.isBottom)
        {
            if(curInning.inningNumber >= numberOfInnings)
            {
                GameOver();
            }
            curInning.inningNumber += 1;
            curInning.isBottom = false;
        }
        else
        {
            if (curInning.inningNumber >= numberOfInnings)
            {
                int teamAtBat = activeTeams[0].currentlyAtBat ? 0 : 1;
                int teamNotBatting = activeTeams[0].currentlyAtBat ? 1 : 0;
                if (activeTeams[teamAtBat].score < activeTeams[teamNotBatting].score)
                {
                    GameOver();
                }
            }
            curInning.isBottom = true;
        }

        SwitchTeamAtBat();
        Debug.Log("Inning reset");
        outs = 0;
    }

    void GameOver()
    {
        ActiveTeam winner = activeTeams[0].score > activeTeams[1].score ? activeTeams[0] : activeTeams[1];
        Debug.Log("Winner is: " + winner.name);
    }
}
