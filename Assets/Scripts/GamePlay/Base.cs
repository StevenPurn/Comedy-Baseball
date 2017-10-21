using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {

    public bool isOccupied;
    public GameObject baseObj;

    private void Start()
    {
        baseObj = this.gameObject;
    }

}
