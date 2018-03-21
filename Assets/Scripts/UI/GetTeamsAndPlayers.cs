using UnityEngine.UI;
using UnityEngine;
using System;
using System.IO;

public class GetTeamsAndPlayers : MonoBehaviour {

    [Header("Home Team")]
    public Text homeTeamName;
    public Text homePlayer1Name, homePlayer2Name, homePlayer3Name;
    public Image homePlayer1Img, homePlayer2Img, homePlayer3Img;
    public Text homeTeamRecord;

    [Header("Away Team")]
    public Text awayTeamName;
    public Text awayPlayer1Name, awayPlayer2Name, awayPlayer3Name;
    public Image awayPlayer1Img, awayPlayer2Img, awayPlayer3Img;
    public Text awayTeamRecord;

    [Header("Other")]
    public Text matchupText;

    //Eventually use this to set up the pregame screen
    void Start () {

        ActiveTeam homeTeam = GameControl.instance.activeTeams[0];
        ActiveTeam awayTeam = GameControl.instance.activeTeams[1];
        //Set up home team
        homeTeamName.text = homeTeam.name;
        homePlayer1Img.sprite = GetPortaitForPlayer(homeTeam.players[0]);
        homePlayer1Name.text = homeTeam.players[0].name;
        homePlayer2Img.sprite = GetPortaitForPlayer(homeTeam.players[1]);
        homePlayer2Name.text = homeTeam.players[1].name;
        homePlayer3Img.sprite = GetPortaitForPlayer(homeTeam.players[2]);
        homePlayer3Name.text = homeTeam.players[2].name;
        homeTeamRecord.text = homeTeam.wins + "-" + homeTeam.loses;

        //Set up away team
        awayTeamName.text = awayTeam.name;
        awayPlayer1Img.sprite = GetPortaitForPlayer(awayTeam.players[0]);
        awayPlayer1Name.text = awayTeam.players[0].name;
        awayPlayer2Img.sprite = GetPortaitForPlayer(awayTeam.players[1]);
        awayPlayer2Name.text = awayTeam.players[1].name;
        awayPlayer3Img.sprite = GetPortaitForPlayer(awayTeam.players[2]);
        awayPlayer3Name.text = awayTeam.players[2].name;
        awayTeamRecord.text = awayTeam.wins + "-" + awayTeam.loses;
    }

    private Sprite GetPortaitForPlayer(ActivePlayer player)
    {
        string path = Application.dataPath + player.portraitPath;
        byte[] data = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        texture.LoadImage(data);
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}