using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BetweenInnings : MonoBehaviour {

    public List<InputField> suggestions = new List<InputField>();
    public GameObject betweenInningUI;
    public Text suggestionsText;

    private void Awake()
    {
        //turn off input listening for the numbers
    }

    void Update () {
        if (Controls.GetButtonDown("escape"))
        {
            suggestionsText.text = "";
            GameControl.curInning.suggestions.Clear();
            int i = 0;
            foreach (var suggestion in suggestions)
            {
                if(suggestion.text != "")
                {
                    suggestionsText.text += i == 0 ? "-" + suggestion.text : Environment.NewLine + "-" + suggestion.text;
                }
                i++;
            }
            betweenInningUI.SetActive(false);
        }
	}
}
