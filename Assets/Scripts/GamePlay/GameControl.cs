using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    //Fake singleton implementation
    public static GameControl instance;

    public static int numberOfInnings = 3;
    public static Inning curInning;
    public string teamFilePath = "/Data/Teams.xml";
    public string playerFilePath = "/Data/Players.xml";
    //Teams & players that exist in the xml files
    public List<Team> teams = new List<Team>();
    public List<Player> players = new List<Player>();
    //Teams/players participating in this game
    public List<ActiveTeam> activeTeams = new List<ActiveTeam>();
    public List<ActivePlayer> activePlayers = new List<ActivePlayer>();

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
    }
	
    //Save list of teams & players to respective xml files
    void Save()
    {
        //Need to update the list based on data from Active lists before saving
        SaveData(teamFilePath, teams);
        SaveData(playerFilePath, players);
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

    //Save data to xml file
    void SaveData(string filePath, System.Object obj)
    {
        //Add new data to active teams & players
        DataHandler.SaveData(filePath, obj);
    }

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

    //Respond to hits based on input from Umpire
    //Should add enum for on base/fly out/ground out
    //Can also switch to next batter or next team if there are 3 outs
    public void HandleHit(int bases)
    {
        switch (bases)
        {
            case 1:
                Debug.Log("Player hit a single");
                break;
            case 2:
                Debug.Log("Player hit a double");
                break;
            case 3:
                Debug.Log("Player hit a triple");
                break;
            case 4:
                Debug.Log("Player hit a homerun");
                break;
            default:
                Debug.Log("Invalid hit");
                break;
        }
    }

    public void NextBatter()
    {
        int teamAtBat;

        if (activeTeams[0].currentlyAtBat)
        {
            teamAtBat = 0;
        }
        else
        {
            teamAtBat = 1;
        }

        int curBatter = GetCurrentBatter();
        if(curBatter + 2 > activeTeams[teamAtBat].players.Count)
        {
            curBatter = 0;
        }

        activeTeams[teamAtBat].players[curBatter].isAtBat = true;
    }

    int GetCurrentBatter()
    {
        int teamAtBat;
        if (activeTeams[0].currentlyAtBat)
        {
            teamAtBat = 0;
        }
        else
        {
            teamAtBat = 1;
        }

        for (int i = 0; i < activeTeams[teamAtBat].players.Count; i++)
        {
            if (activeTeams[teamAtBat].players[i].isAtBat)
            {
                return i;
            }
        }
        return 0;
    }

    void SwitchTeamAtBat()
    {
        activeTeams[0].currentlyAtBat = !activeTeams[0].currentlyAtBat;
        activeTeams[1].currentlyAtBat = !activeTeams[1].currentlyAtBat;

        if(activeTeams[0].currentlyAtBat && activeTeams[1].currentlyAtBat)
        {
            Debug.LogWarning("Both teams are batting");
        }
    }
}
