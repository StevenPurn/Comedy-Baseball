using UnityEngine.UI;
using UnityEngine;
using System;
using System.IO;

public class UIControl : MonoBehaviour {

    public Text inning;
    public Text batter;
    public Image[] outs;
    public Image[] inningIndicators;
    public Image[] bases;
    public Sprite activeOut, inactiveOut, occupiedBase, unoccupiedBase, activeInning, inactiveInning;
    public Image batterPortrait;

    void Start () {
        GameControl.instance.changeCountEvent += UpdateUI;
        UpdateUI();
    }
	
	void UpdateUI()
    {
        UpdateBatterUI();
        UpdateCountUI();
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
        Debug.Log(batAvg);
        Debug.Log(String.Format("{0:.000}", batAvg));
        string batText = curBatter.number.ToString() + "\n" + curBatter.name + "\n." + batAvg;
        batter.text = batText;
    }

    void UpdateCountUI()
    {
        inningIndicators[0].sprite = GameControl.curInning.isBottom ? inactiveInning : activeInning;
        inningIndicators[1].sprite = GameControl.curInning.isBottom ? activeInning : inactiveInning;
        inning.text = GameControl.curInning.inningNumber.ToString();
        foreach (var item in outs)
        {
            item.sprite = inactiveOut;
        }

        foreach (var item in bases)
        {
            item.sprite = unoccupiedBase;
        }

        //This needs to be fixed
        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].sprite = Field.bases[i + 1].isOccupied ? occupiedBase : unoccupiedBase;
        }

        for (int i = 0; i < GameControl.outs; i++)
        {
            outs[i].sprite = activeOut;
        }
    }
}
