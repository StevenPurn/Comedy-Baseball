using UnityEngine;

public class Indestructible : MonoBehaviour {

	void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
}
