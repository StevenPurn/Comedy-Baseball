using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HRDMenuControl : MonoBehaviour {

    public Text[] playerNames = new Text[10];

    public void SetPlayers()
    {
        if (NamesValid())
        {
            string[] names = new string[10];
            for (int i = 0; i < playerNames.Length; i++)
            {
                names[i] = playerNames[i].text;
            }
            HRDGameControl.SetPlayers(names);

            GetComponent<SceneLoader>().LoadScene("HRDWaitForSpaceBar");
        }
    }

    private bool NamesValid()
    {
        List<string> names = new List<string>();
        int count = 0;
        foreach (var item in playerNames)
        {
            if (item.text == "")
            {
                count += 1;
            }
            else
            {
                if (names.Contains(item.text))
                {
                    return false;
                }
                names.Add(item.text);
            }
        }
        return true;
    }
}
