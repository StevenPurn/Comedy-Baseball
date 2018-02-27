using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    public float curHeight = 1.5f;
    public bool popFly = false;
    //Chance of error (possibly from previous play)
    //Update the angles of the hits to be more human readable
    private BoxCollider2D col;
    public Pitch curPitch;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        TemporarilyDisableCollision(2.0f);
        curHeight = 10f;
    }

    public void AddRelativeForceToBall(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        if (force.magnitude > 5.0f)
        {
            popFly = true;
        }
        rb.AddRelativeForce(force, ForceMode2D.Impulse);
    }

    public void DeterminePitchResults()
    {
        //This needs to be randomized, iterate through list and randomly pick one result based on probability
        float total = 0f;
        foreach(KeyValuePair<float, Pitcher.Pitches> entry in curPitch.types)
        {
            total += entry.Key;
        }
        float randType = Random.Range(0f, total);
        float curTotal = 0f;
        foreach (KeyValuePair<float, Pitcher.Pitches> entry in curPitch.types)
        {
            curTotal += entry.Key;
            if(randType <= curTotal)
            {
                curPitch.type = entry.Value;
                Debug.Log(curPitch.type);
                break;
            }
        }

        curPitch.hitAngle = curPitch.hitAngles[Random.Range(0, curPitch.hitAngles.Count)];
        curPitch.hitSpeed = Random.Range(curPitch.minSpeed, curPitch.maxSpeed);
    }

    public void AddForceToBall(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void HitBallWithFuckingBat(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        Vector2 pos = transform.position;
        rb.AddForce(pos - force, ForceMode2D.Impulse);
    }

    public void TemporarilyDisableCollision()
    {
        col.enabled = false;
        Invoke("EnableCollision", 0.2f);
    }

    public void TemporarilyDisableCollision(float timeDelay)
    {
        col.enabled = false;
        Invoke("EnableCollision", timeDelay);
    }

    public void EnableCollision()
    {
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.transform.tag;
        if (tag == "Fielder")
        {
            if (curHeight < 4.0f)
            {
                collision.GetComponentInParent<Fielder>().ballInHands = true;
                rb.velocity = Vector2.zero;
            }
        }else if(tag == "Runner")
        {
            collision.GetComponent<Runner>().SwingBat(curPitch.type == Pitcher.Pitches.strike);
        }
    }
}
