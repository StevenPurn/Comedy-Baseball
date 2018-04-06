using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class GameOverUI : MonoBehaviour {

    public Text gameOverText;
    public Image player1Portrait, player2Portrait, player3Portrait;
    public Text player1Name, player2Name, player3Name;

	// Use this for initialization
	void Start () {
        ActiveTeam winningTeam = GameControl.instance.activeTeams[0].wonGame ? GameControl.instance.activeTeams[0] : null;
        if(winningTeam == null)
        {
            winningTeam = GameControl.instance.activeTeams[1].wonGame ? GameControl.instance.activeTeams[1] : null;
        }
        if(winningTeam == null)
        {
            gameOverText.text = "They tied. That sucks. Hopefully it was funny anyway.";
            player1Portrait.color = new Color(0,0,0,0);
            player1Name.text = "";
            player2Portrait.color = new Color(0, 0, 0, 0);
            player2Name.text = "";
            player3Portrait.color = new Color(0, 0, 0, 0);
            player3Name.text = "";
        }
        else
        {
            gameOverText.text = winningTeam.name + " won!";
            SetBatterPhoto(winningTeam.players[0], player1Portrait);
            player1Name.text = winningTeam.players[0].name;
            SetBatterPhoto(winningTeam.players[1], player2Portrait);
            player2Name.text = winningTeam.players[1].name;
            SetBatterPhoto(winningTeam.players[2], player3Portrait);
            player3Name.text = winningTeam.players[2].name;
        }
	}

    void SetBatterPhoto(ActivePlayer player, Image img)
    {
        string path = Application.dataPath + player.portraitPath;
        byte[] data = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        texture.LoadImage(data);
        img.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
