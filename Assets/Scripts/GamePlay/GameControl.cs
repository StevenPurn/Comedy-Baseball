using System;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    //Fake singleton implementation
    public static GameControl instance;
    //Static variables
    public static int numberOfInnings = 3;
    public static Inning curInning = new Inning();
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
    private int i;
    private int teamAtBat;

    public Dictionary<TeamColor, Color> teamColors = new Dictionary<TeamColor, Color>()
    {
        {TeamColor.red, Color.red },
        {TeamColor.blue, Color.blue }
    };

    public delegate void ChangeCount();
    public ChangeCount changeCountEvent;

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
            Team _team = teams.Find(x => x.name == aTeam.name);
            if(_team != null)
            {
                UpdateTeamData(aTeam, _team);
            }
        }

        foreach (var aPlayer in activePlayers)
        {
            Player _player = players.Find(x => x.name == aPlayer.name);
            if (_player != null)
            {
                UpdatePlayerData(aPlayer, _player);
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
    }

    void NextBatter()
    {
        int curBatter = GetCurrentBatter();
        curBatter = curBatter + 1 == activeTeams[teamAtBat].players.Count ? 0 : curBatter + 1;

        foreach (var player in activeTeams[teamAtBat].players)
        {
            player.isAtBat = false;
        }

        foreach (var runner in runners)
        {
            runner.atBat = false;
        }

        AddBatterToField();
        activeTeams[teamAtBat].players[curBatter].isAtBat = true;
    }

    public static void InitializeField()
    {
        instance.teamAtBat = instance.activeTeams[0].currentlyAtBat ? 0 : 1;
        instance.AddBatterToField();
    }

    public void AddBatterToField()
    {
        if (battersBox == null)
        {
            battersBox = GameObject.Find("BattersBox").transform;
        }
        GameObject go = Instantiate(runnerPrefab, battersBox.position, Quaternion.identity);
        go.name = "Runner " + i;
        i += 1;
        Field.runners.Add(go.GetComponent<Runner>());
    }

    int GetCurrentBatter()
    {
        if(activeTeams[teamAtBat].players.Find(x => x.isAtBat == true) != null)
        {
            return activeTeams[teamAtBat].players.FindIndex(x => x.isAtBat == true);
        }
        //By default return 0, should only occur with first batter of the game
        return 0;
    }

    public ActivePlayer GetCurrentBattingPlayer()
    {
        if (activeTeams[teamAtBat].players.Find(x => x.isAtBat == true) == null)
        {
            return activeTeams[teamAtBat].players[0];
        }
        else
        {
            return activeTeams[teamAtBat].players.Find(x => x.isAtBat == true);
        }
    }

    void SwitchTeamAtBat()
    {
        activeTeams[0].currentlyAtBat = !activeTeams[0].currentlyAtBat;
        activeTeams[1].currentlyAtBat = !activeTeams[1].currentlyAtBat;

        teamAtBat = activeTeams[0].currentlyAtBat ? 0 : 1;

        if (activeTeams[0].currentlyAtBat && activeTeams[1].currentlyAtBat)
        {
            Debug.LogWarning("Both teams are batting");
        }else if(!activeTeams[0].currentlyAtBat && !activeTeams[1].currentlyAtBat)
        {
            Debug.LogWarning("Neither team is batting");
        }
        changeCountEvent();
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
        changeCountEvent();
    }

    //Need to add potential for advancing runners on base during an out
    public void HandleOut()
    {
        outs += 1;
        ResetCount();
        NextBatter();
        changeCountEvent();
        if (outs >= 3)
        {
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
        changeCountEvent();
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
        GetCurrentBattingPlayer().hits += 1;
        Field.AdvanceRunners(bases);
        NextBatter();
        ResetCount();
    }

    public void ChangeTeamScore(int change)
    {
        activeTeams[teamAtBat].score += change;
        changeCountEvent();
    }

    #endregion

    void ResetCount()
    {
        balls = 0;
        strikes = 0;
        changeCountEvent();
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
                int teamNotBatting = teamAtBat == 0 ? 1 : 0;
                if (activeTeams[teamAtBat].score < activeTeams[teamNotBatting].score)
                {
                    GameOver();
                }
            }
            curInning.isBottom = true;
        }

        SwitchTeamAtBat();
        Field.ResetInning();
        outs = 0;
        AddBatterToField();
    }

    void GameOver()
    {
        ActiveTeam winner = activeTeams[0].score > activeTeams[1].score ? activeTeams[0] : activeTeams[1];
        Debug.Log("Winner is: " + winner.name);
    }
}
