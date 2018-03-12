using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    public float curHeight = 1.5f;
    public bool popFly = false;
    public float maxHeight, curSpeed;
    public Vector2 endPoint, startPoint;
    private BoxCollider2D col;
    public Pitch curPitch;
    public Animator anim;
    //Chance of error (possibly from previous play)
    //Update the angles of the hits to be more human readable

    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        TemporarilyDisableCollision();
        curHeight = 2f;
    }

    private void Update()
    {
        if (Utility.CheckEqual(transform.position, endPoint, 0.05f))
        {
            anim.SetBool("Moving", false);
            endPoint = Vector3.zero;
        }
        if(endPoint != Vector2.zero)
        {
            MoveBall(endPoint);
        }

        float scale = Mathf.Clamp(curHeight / 4, 0.5f, 4f);
        transform.localScale = new Vector3(scale, scale);
    }

    public void MoveBall(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(transform.position + direction * curSpeed * Time.deltaTime);
        Vector2 halfwayPoint = (endPoint - startPoint) / 2 + startPoint;
        curHeight = maxHeight * (1 - Mathf.Abs(Vector2.Distance(transform.position, halfwayPoint) / Vector2.Distance(startPoint, halfwayPoint)));
        anim.SetBool("Moving", true);
    }

    public void DeterminePitchResults()
    {
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
                break;
            }
        }
        curPitch.hitSpeed = Random.Range(curPitch.minSpeed, curPitch.maxSpeed);
    }

    public void TemporarilyDisableCollision(float timeDelay = 3.5f)
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
                if (collision.GetComponentInParent<Fielder>().ballInHands == false)
                {
                    string aud = "catch";
                    AudioControl.instance.PlayAudio(aud);
                }
                collision.GetComponentInParent<Fielder>().ballInHands = true;
                endPoint = Vector3.zero;
                startPoint = Vector3.zero;
                anim.SetBool("Moving", false);
            }
        }else if(tag == "Runner")
        {
            collision.GetComponent<Runner>().SwingBat(curPitch.type == Pitcher.Pitches.strike);
        }
    }
}
