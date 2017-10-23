using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CountText : MonoBehaviour {

    public Text count;
    public Text batter;

	// Use this for initialization
	void Start () {
        GameControl.instance.changeCountEvent += UpdateText;
        UpdateText();
    }
	
	// Update is called once per frame
	void UpdateText ()
    {
        UpdateBatterText();
        UpdateCountText();
	}

    void UpdateBatterText()
    {
        string batterText = String.Format("{0}\nHits:", GameControl.instance.GetCurrentBattingPlayer().name);
        batterText += GameControl.instance.GetCurrentBattingPlayer().hits;
        batter.text = batterText;
    }

    void UpdateCountText()
    {
        count.text = String.Format("Outs: {0}\nStrikes:{1}\nBalls:{2}", GameControl.outs, GameControl.strikes, GameControl.balls);
    }
}
