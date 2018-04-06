using System;
using System.Collections.Generic;

public static class SaveLoad
{
    //Save list of teams & players to respective xml files
    public static void Save(List<ActiveTeam> activeTeams, List<Team> teams)
    {
        foreach (var aTeam in activeTeams)
        {
            Team _team = teams.Find(x => x.name == aTeam.name);
            if (_team != null)
            {
                UpdateTeamData(aTeam, _team);
            }

            foreach (var aPlayer in aTeam.players)
            {
                Player _player = GameControl.instance.players.Find(x => x.name == aPlayer.name);
                if (_player != null)
                {
                    UpdatePlayerData(aPlayer, _player);
                }
            }
        }

        SaveData(GameControl.instance.teamFilePath, teams);
        SaveData(GameControl.instance.playerFilePath, GameControl.instance.players);
    }

    static void UpdateTeamData(ActiveTeam aTeam, Team team)
    {
        //Set values of the team to account for updated values in active team
        team.hits += aTeam.hits;
        team.runs += aTeam.score;
        team.atbatstrikeouts += aTeam.GetStrikeoutsAtBat();
        team.pitchedstrikeouts += aTeam.GetPitchedStrikeouts();
        if (aTeam.wonGame)
        {
            team.wins += 1;
        }
        else if(aTeam.lostGame)
        {
            team.loses += 1;
        }else if (aTeam.tiedGame)
        {
            team.ties += 1;
        }
    }

    static void UpdatePlayerData(ActivePlayer aPlayer, Player player)
    {
        player.hits += aPlayer.hits;
        player.atBats += aPlayer.atBats;
        player.rbis += aPlayer.rbis;
        player.strikeoutsAtBat += aPlayer.strikeoutsAtBat;
        player.strikeoutsPitched += aPlayer.strikeoutsPitched;
        player.battingAvg = (float)Math.Round(((float)player.hits / (float)player.atBats), 3);
    }

    //Save data to xml file
    public static void SaveData(string filePath, System.Object obj)
    {
        //Add new data to active teams & players
        DataHandler.SaveData(filePath, obj);
    }

    //Load team data
    public static void PopulateTeamList()
    {
        GameControl.instance.teams = DataHandler.LoadData<List<Team>>(GameControl.instance.teamFilePath, Team.xmlRoot);
    }

    //Load player data
    public static void PopulatePlayerList()
    {
        GameControl.instance.players = DataHandler.LoadData<List<Player>>(GameControl.instance.playerFilePath, Player.xmlRoot);
    }
}
