using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {

    public bool atBat;
    public Base[] bases;
    private GameObject target;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Set next base as a target
    public void AdvanceBase()
    {
        
    }

    //Move player towards next base if they have one
    private void Update()
    {
        
    }

    public void SetBaseAsTarget(int baseToTarget)
    {

    }
}
