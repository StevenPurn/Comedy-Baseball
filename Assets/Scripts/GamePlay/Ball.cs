using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AddForceToBall(Vector2 force)
    {
        rb.AddForce(force);
    }

    void FixedUpdate ()
    {
        //Can potentially be used for throws if you don't want randomly dropped balls or anything
    }
}
