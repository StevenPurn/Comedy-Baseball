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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.transform.tag;
        if (tag == "Fielder")
        {
            Debug.Log("fielder has ball");
            collision.GetComponent<Fielder>().ballInHands = true;
            rb.velocity = Vector2.zero;
        }else if(tag == "Runner")
        {
            collision.GetComponent<Runner>().SwingBat();
        }
    }
}
