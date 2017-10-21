using UnityEngine.UI;
using UnityEngine;
using System;

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
        batter.text = String.Format("{0}\nHits: {1}", GameControl.instance.GetCurrentBattingPlayer().name, GameControl.instance.GetCurrentBattingPlayer().hits);
    }

    void UpdateCountText()
    {
        count.text = String.Format("Outs: {0}\nStrikes:{1}\nBalls:{2}", GameControl.outs, GameControl.strikes, GameControl.balls);
    }
}
