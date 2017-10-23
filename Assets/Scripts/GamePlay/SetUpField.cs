using UnityEngine;

public class SetUpField : MonoBehaviour {

    public Base[] bases = new Base[4];

	// Use this for initialization
	void Awake ()
    {
        GameControl.InitializeField();
        Field.bases = bases;
    }
}
