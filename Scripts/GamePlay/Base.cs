using UnityEngine;

public class Base : MonoBehaviour {

    public bool isOccupied;
    public GameObject baseObj;

    private void Awake()
    {
        baseObj = this.gameObject;
    }
}
