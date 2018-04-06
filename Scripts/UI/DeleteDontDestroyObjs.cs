using UnityEngine;

public class DeleteDontDestroyObjs : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Indestructible[] destroyObjs = FindObjectsOfType<Indestructible>();
        for (int i = 0; i < destroyObjs.Length; i++)
        {
            Destroy(destroyObjs[i].transform.gameObject);
        }
	}
}
