using UnityEngine.UI;
using UnityEngine;
using System;
using System.IO;

public class UIControl : MonoBehaviour {

    public Text inning;
    public Text batter;
    public Text homeScore, awayScore;
    public Text count;
    public Image[] outs;
    public Image[] inningIndicators;
    public Image[] bases;
    public Sprite activeOut, inactiveOut, occupiedBase, unoccupiedBase, activeInning, inactiveInning;
    public Image batterPortrait;

    void Start () {
        GameControl.instance.changeCountEvent += UpdateUI;
        UpdateUI();
        //GameControl.instance.changeCountEvent += UpdateUI;
        //UpdateUI();
    }
	
	void UpdateUI()
    {
        UpdateScoreText();
        UpdateBatterUI();
        UpdateCountUI();
	}

    void UpdateScoreText()
    {
        homeScore.text = GameControl.instance.activeTeams[0].abbreviation + ": " + GameControl.instance.activeTeams[0].score;
        awayScore.text = GameControl.instance.activeTeams[1].abbreviation + ": " + GameControl.instance.activeTeams[1].score;
    }

    void UpdateBatterUI()
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
        string batAvg = ((float)Math.Round(((float)curBatter.totalHits / (float)curBatter.totalAtBats), 3) * 1000).ToString();
        if (curBatter.totalHit < 1)
        {
            batAvg = "000";
        }
        string batText = curBatter.number.ToString() + "\n" + curBatter.name + "\n." + batAvg;
        batter.text = batText;
    }

    void UpdateCountUI()
    {
        inningIndicators[0].sprite = GameControl.curInning.isBottom ? inactiveInning : activeInning;
        inningIndicators[1].sprite = GameControl.curInning.isBottom ? activeInning : inactiveInning;
        inning.text = GameControl.curInning.inningNumber.ToString();
        count.text = GameControl.balls.ToString() + "-" + GameControl.strikes.ToString();
        foreach (var item in outs)
        {
            item.sprite = inactiveOut;
        }

        for (int i = 0; i < GameControl.outs; i++)
        {
            outs[i].sprite = activeOut;
        }
    }

    public void UpdateBaseIndicators()
    {
        foreach (var item in bases)
        {
            item.sprite = unoccupiedBase;
        }

        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].sprite = Field.bases[i + 1].isOccupied ? occupiedBase : unoccupiedBase;
        }
    }
}
