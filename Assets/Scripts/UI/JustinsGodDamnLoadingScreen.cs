using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JustinsGodDamnLoadingScreen : MonoBehaviour {

    public Text loadingText;
    private float textTimer;

	// Use this for initialization
	void Start () {
        loadingText = GetComponent<Text>();
        textTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        textTimer += Time.deltaTime;

        if(textTimer > 0.5f)
        {
            textTimer = 0.0f;
            if(loadingText.text == "Loading...")
            {
                loadingText.text = "Loading";
            }
            else
            {
                loadingText.text += ".";
            }
        }
	}
}
