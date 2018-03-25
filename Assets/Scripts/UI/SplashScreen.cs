using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    public float timeElapsed;
    public string sceneName;

	// Use this for initialization
	void Start () {
        timeElapsed = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timeElapsed += Time.deltaTime;

        if(timeElapsed > 8.0f)
        {
            SceneManager.LoadScene(sceneName);
        }
	}
}
