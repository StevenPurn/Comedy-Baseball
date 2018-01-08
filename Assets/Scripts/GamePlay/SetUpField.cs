﻿using UnityEngine;

public class SetUpField : MonoBehaviour {

    public Base[] bases = new Base[4];
    public GameObject[] dugouts = new GameObject[2];

	// Use this for initialization
	void Awake ()
    {
        Field.bases = bases;
        Field.dugouts = dugouts;
        Field.AssignDugouts();
        GameControl.InitializeField();
    }
}
