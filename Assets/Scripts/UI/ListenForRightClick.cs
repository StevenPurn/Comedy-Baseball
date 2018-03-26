using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenForRightClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnMouseOver()
    {
        Debug.Log("I'm over " + this.gameObject.name);
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("clicked on " + this.gameObject.name);
        }
    }
}
