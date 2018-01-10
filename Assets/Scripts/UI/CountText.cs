using UnityEngine.UI;
using UnityEngine;
using System;

public class CountText : MonoBehaviour {

    public Text inning;
    public Text batter;
    public Image[] outs;
    public Image[] inningIndicators;
    public Image[] bases;
    public Sprite activeOut, inactiveOut, occupiedBase, unoccupiedBase, activeInning, inactiveInning;

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
        string batterText = String.Format("{0}\nHits:", GameControl.instance.GetCurrentBattingPlayer().name);
        batterText += GameControl.instance.GetCurrentBattingPlayer().hits;
        //batter.text = batterText;
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
            bases[i].sprite = Field.runners.Find(x => x.currentBase == i) != null ? occupiedBase : unoccupiedBase;
        }

        for (int i = 0; i < GameControl.outs; i++)
        {
            outs[i].sprite = activeOut;
        }
    }
}
