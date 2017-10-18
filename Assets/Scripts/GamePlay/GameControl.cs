using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static int numberOfInnings = 3;
    public static GameControl instance;
    public string teamFilePath = "/Data/Teams.xml";
    public string playerFilePath = "/Data/Players.xml";
    public List<Team> teams = new List<Team>();
    public List<Player> players = new List<Player>();
    public List<ActiveTeam> activeTeams = new List<ActiveTeam>();
    public List<ActivePlayer> activePlayers = new List<ActivePlayer>();

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
        Save();
    }
	
    void Save()
    {
        SaveData(teamFilePath, teams);
        SaveData(playerFilePath, players);
    }

    void PopulateTeamList()
    {
        teams = DataHandler.LoadData<List<Team>>(teamFilePath, Team.xmlRoot);
    }

    void PopulatePlayerList()
    {
        players = DataHandler.LoadData<List<Player>>(playerFilePath, Player.xmlRoot);
    }

    void SaveData(string filePath, System.Object obj)
    {
        DataHandler.SaveData(filePath, obj);
    }
}
