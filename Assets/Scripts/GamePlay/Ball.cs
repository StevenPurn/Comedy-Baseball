﻿using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    public float curHeight = 1.5f;
    public bool hasntHitGround = false;
    public float maxHeight, curSpeed;
    public Vector2 endPoint, startPoint;
    private BoxCollider2D col;
    public Pitch curPitch;
    public Animator anim;
    public Fielder targetFielder;
    //Chance of error (possibly from previous play)
    //Update the angles of the hits to be more human readable

    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        curHeight = 2f;
    }

    private void Update()
    {
        if (Utility.CheckEqual(transform.position, endPoint, 0.03f))
        {
            Debug.Log("reached endpoint");
            hasntHitGround = false;
            //anim.SetBool("Moving", false);
            curSpeed = curSpeed / 2;
            if(curSpeed < 0.1f)
            {
                maxHeight = 1;
                endPoint = Vector3.zero;
            }
            else
            {
                maxHeight = maxHeight / 4;
                float xPos = Field.bases[0].transform.position.x - transform.position.x;
                float yPos = Field.bases[0].transform.position.y - transform.position.y;
                //These need to be better approxiate locations you fool.
                endPoint = new Vector3(xPos, yPos, 0);
            } 
        }
        if(endPoint != Vector2.zero)
        {
            MoveBall(endPoint);
        }

        float scale = Mathf.Clamp(curHeight / 4, 0.5f, 4f);
        transform.localScale = new Vector3(scale, scale);
        Field.ballLandingSpot = endPoint;
        if(curPitch != null)
        {
            bool ignore = false;
            if(curPitch.type == Pitcher.Pitches.homerun)
            {
                ignore = true;
            }
            Physics.IgnoreLayerCollision(0, 8, ignore);
        }
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

    public void HandleHomeRun(float timeDelay = 10f)
    {
        TemporarilyDisableCollision(timeDelay);
        Invoke("ReturnToPitcher", timeDelay);
    }

    public void TemporarilyDisableCollision(float timeDelay = 3.5f)
    {
        col.enabled = false;
        Invoke("EnableCollision", timeDelay);
    }

    private void ReturnToPitcher()
    {
        anim.SetBool("Moving", false);
        Fielder pitcher = Field.fielders.Find(x => x.position == Fielder.Position.pitcher);
        endPoint = Vector2.zero;
        pitcher.ballInHands = true;
    }

    public void EnableCollision()
    {
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.transform.tag;
        if (tag == "Runner")
        {
            collision.GetComponent<Runner>().SwingBat(curPitch.type == Pitcher.Pitches.strike);
        }else if(curPitch != null && tag == "Fielder")
        {
            if (curPitch.type != Pitcher.Pitches.homerun)
            {
                if (GameControl.curInning.pitchesThrownThisInning > 0)
                {
                    if (curHeight < 4.0f)
                    {
                        //if (hasntHitGround)
                        //{
                        //    Field.mostRecentBatter.SetOut();
                        //    GameControl.instance.HandleOut();
                        //    string aud = "out" + UnityEngine.Random.Range(1, 5);
                        //    AudioControl.instance.PlayAudio(aud);
                        //}
                        hasntHitGround = false;
                        if (collision.GetComponentInParent<Fielder>().ballInHands == false)
                        {
                            string aud = "catch";
                            AudioControl.instance.PlayAudio(aud);
                        }
                        collision.GetComponentInParent<Fielder>().ballInHands = true;
                        targetFielder = null;
                        endPoint = Vector3.zero;
                        startPoint = Vector3.zero;
                        anim.SetBool("Moving", false);
                    }
                }
            }
        }else if(tag == "Wall" && curPitch.type != Pitcher.Pitches.homerun)
        {
            startPoint = transform.position;
            endPoint = transform.position + new Vector3(Random.Range(-0.5f, 0.5f),Random.Range(-0.5f, -1f),0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.transform.tag;
        if (tag == "Fielder")
        {
            collision.GetComponentInParent<Fielder>().ballInHands = false;
        }
    }
}
