using UnityEngine.UI;
using UnityEngine;
using System;

public class ScoreText : MonoBehaviour {

    public Text score;

	// Use this for initialization
	void Start () {
        GameControl.instance.changeCountEvent += UpdateText;
        UpdateText();
    }
	
	// Update is called once per frame
	void UpdateText ()
    {

        UpdateScoreText();
	}

    void UpdateScoreText()
    {
        score.text = String.Format("Inning: {0}\n{1}: {2}\n{3}: {4}", GameControl.curInning.inningNumber, GameControl.instance.activeTeams[0].name, GameControl.instance.activeTeams[0].score, GameControl.instance.activeTeams[1].name, GameControl.instance.activeTeams[1].score);
    }
}
