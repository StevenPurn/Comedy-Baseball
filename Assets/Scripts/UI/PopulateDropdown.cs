using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Dropdown))]
public class PopulateDropdown : MonoBehaviour {

    public string type;
    private Dropdown dropDown;

	//Add all items from xml files to the dropdown lists
	void Start () {
        dropDown = GetComponent<Dropdown>();
        List<string> items = new List<string>();

        switch (type)
        {
            case "team":
                foreach (var item in GameControl.instance.teams)
                {
                    items.Add(item.name);
                }
                break;
            case "player":
                foreach (var item in GameControl.instance.players)
                {
                    items.Add(item.name);
                }
                break;
            default:
                Debug.Log("shit's fucked yo");
                break;
        }

        dropDown.AddOptions(items);
    }
}
