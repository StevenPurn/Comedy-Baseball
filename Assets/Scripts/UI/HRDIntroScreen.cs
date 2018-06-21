using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class HRDIntroScreen : MonoBehaviour {

    public List<Text> textFields = new List<Text>();

	void Start () {
        for (int i = 0; i < textFields.Count; i++)
        {
            textFields[i].text = HRDGameControl.players[i].name;
        }	
	}
}
