using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BetweenInnings : MonoBehaviour {

    public List<InputField> suggestions = new List<InputField>();
    public GameObject betweenInningUI;
	void Update () {
        if (Controls.GetButtonDown("escape"))
        {
            GameControl.curInning.suggestions.Clear();
            foreach (var suggestion in suggestions)
            {
                if(suggestion.text != "")
                {
                    GameControl.curInning.suggestions.Add(suggestion.text);
                }
            }
            foreach (var item in GameControl.curInning.suggestions)
            {
                Debug.Log(item);
            }
            betweenInningUI.SetActive(false);
        }
	}
}
