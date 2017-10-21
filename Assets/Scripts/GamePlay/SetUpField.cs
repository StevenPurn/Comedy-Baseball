using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpField : MonoBehaviour {

    public Base[] bases = new Base[4];

	// Use this for initialization
	void Start ()
    {
        Field.bases = bases;
        GameControl.instance.AddBatterToField();
	}
}
