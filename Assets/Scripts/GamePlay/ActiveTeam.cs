using System.Collections.Generic;
using UnityEngine;

public class ActiveTeam
{
    public string name;
    public string abbreviation;
    public List<ActivePlayer> players = new List<ActivePlayer>();
    public GameObject dugout;
    public Inning[] innings = new Inning[GameControl.numberOfInnings];
    public int score = 0;
    public int totalScore;
    public int hits;
    public int totalHits;
    public int wins;
    public int loses;
    public bool currentlyAtBat;
    public bool wonGame;
    public Color color;

    public void AddPlayer(ActivePlayer player)
    {
        if (players.Contains(player))
        {
            Debug.LogWarning("Tried to add player already on the team");
        }
        else
        {
            players.Add(player);
        }
    }

    public void RemovePlayer(ActivePlayer player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
        else
        {
            Debug.LogWarning("Tried to remove player not on the team");
        }
    }

    public void ChangeScore(int change, int inning)
    {
        score += change;
        innings[inning].score += change;
    }

    public int GetHits()
    {
        hits = 0;
        foreach (var player in players)
        {
            hits += player.hits;
        }

        return hits;
    }

    public int GetStrikeoutsAtBat()
    {
        int strikeouts = 0;
        foreach (var player in players)
        {
            strikeouts += player.strikeoutsAtBat;
        }

        return strikeouts;
    }

    public int GetPitchedStrikeouts()
    {
        int strikeouts = 0;
        foreach (var player in players)
        {
            strikeouts += player.strikeoutsPitched;
        }

        return strikeouts;
    }
}