using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System;

public class ScoreText : MonoBehaviour {

    public Text batterInfo;
    public Image batterPortrait;

	// Use this for initialization
	void Start () {
        GameControl.instance.changeCountEvent += UpdateBatterPanel;
        UpdateBatterPanel();
    }
	
	// Update is called once per frame
	void UpdateBatterPanel ()
    {
        UpdateBatterText();
        UpdateBatterPhoto();
	}

    void UpdateBatterPhoto()
    {
        //Should update this for when the game is built so it can point anywhere
        string path = Application.dataPath + GameControl.instance.GetCurrentBattingPlayer().portraitPath;
        byte[] data = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        texture.LoadImage(data);
        batterPortrait.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void UpdateBatterText()
    {
        ActivePlayer curBatter = GameControl.instance.GetCurrentBattingPlayer();
        string batText = curBatter.number.ToString() + "\n" + curBatter.name + "\n" + (float)Math.Round(((float)curBatter.totalHits / (float)curBatter.totalAtBats), 3);
        batterInfo.text = batText;
    }
}
