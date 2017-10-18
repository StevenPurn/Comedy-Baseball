using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class GameControl : MonoBehaviour {

    public static int numberOfInnings;
    public static GameControl instance;
    public string teamFilePath, playerFilePath;
    public List<Team> teams = new List<Team>();
    public List<Player> players = new List<Player>();

	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Debug.LogWarning("Multiple Game Controls");
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
