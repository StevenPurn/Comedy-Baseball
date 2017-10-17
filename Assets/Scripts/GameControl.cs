using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static int numberOfInnings;
    public static GameControl instance;


	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Debug.LogWarning("Multiple Game Controls");
        }
        else
        {
            instance = this;
        }
	}
	
    void PopulateTeamList()
    {

    }

    void PopulatePlayerList()
    {
        
    }

	// Update is called once per frame
	void Update () {
		
	}
}
