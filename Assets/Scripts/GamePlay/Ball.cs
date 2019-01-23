using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    public float curHeight = 1.5f;
    public bool hasHitGround = false, wasHit = false;
    public float maxHeight, curSpeed;
    public Vector2 endPoint, startPoint;
    private BoxCollider2D col;
    public Pitch curPitch;
    public Animator anim;
    public Fielder targetFielder;
    public bool shownHomeRunPopup = false;
    //Chance of error (possibly from previous play)
    //Update the angles of the hits to be more human readable
    //Set the origin point and compare the pos to that (for bouncing off walls)

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        curHeight = 2f;
    }

    private void FixedUpdate()
    {
        if (curSpeed < 1f)
        {
            maxHeight = 1;
            endPoint = Vector3.zero;
            anim.SetBool("Moving", false);
            curSpeed = 0;
        }
        if (Utility.CheckEqual(transform.position, endPoint, 0.07f) || curHeight < 0)
        {
            hasHitGround = true;
            curSpeed = curSpeed / 3f;
            maxHeight = maxHeight / 4f;
            if(Field.ballHasBeenThrown == false)
            {
                Vector2 dir = (endPoint - startPoint);
                float rads = Mathf.Atan2(dir.y, dir.x);
                Vector2 newLandingPoint = new Vector2(Mathf.Cos(rads), Mathf.Sin(rads));
                endPoint = endPoint + newLandingPoint;
                startPoint = transform.position;
            }
        }
        if(endPoint != Vector2.zero)
        {
            MoveBall(endPoint);
            rb.velocity = Vector2.zero;
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

        if(targetFielder != null)
        {
            curSpeed = targetFielder.throwSpeed;
        }
    }

    public void MoveBall(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(transform.position + direction * curSpeed * Time.fixedDeltaTime);
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

    public void HandleHomeRun(float timeDelay = 9.5f)
    {
        TemporarilyDisableCollision(timeDelay);
        Invoke("ReturnToPitcher", timeDelay);
    }

    public void HandleFoul(float timeDelay = 3.5f)
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
        GetComponent<SpriteRenderer>().enabled = true;
        GameControl.ballInPlay = false;
        anim.SetBool("Moving", false);
        Fielder pitcher = Field.fielders.Find(x => x.position == Fielder.Position.pitcher && x.inningOver == false);
        endPoint = pitcher.glove.position;
        pitcher.ballInHands = true;
        wasHit = false;
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
            if(targetFielder != null && targetFielder.position == Fielder.Position.catcher)
            {
                collision.GetComponent<Runner>().SwingBat(curPitch.type == Pitcher.Pitches.strike);
            }
        }else if(tag == "Fielder")
        {
            if (curPitch != null && curPitch.type != Pitcher.Pitches.homerun)
            {
                if (GameControl.curInning.pitchesThrownThisInning > 0)
                {
                    if (curHeight < 4.0f)
                    {
                        if (hasHitGround == false && wasHit)
                        {
                            Field.mostRecentBatter.SetOut();
                            GameControl.instance.HandleOut();
                            string aud = "out" + Random.Range(1, 5);
                            AudioControl.instance.PlayAudio(aud);
                            TextPopUps.instance.ShowPopUp("out");
                        }
                        hasHitGround = true;
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
                else
                {
                    Field.fielders.Find(x => x.position == Fielder.Position.pitcher && x.inningOver == false).ballInHands = true;
                }
            }
        }else if(tag == "Wall" && curPitch.type != Pitcher.Pitches.homerun)
        {
            startPoint = transform.position;
            endPoint = transform.position + new Vector3(Random.Range(-0.25f, 0.25f),Random.Range(-0.5f, -0.05f),0);
        } else if(tag == "Wall" && curPitch.type == Pitcher.Pitches.homerun && shownHomeRunPopup == false)
        {
            shownHomeRunPopup = true;
            AudioControl.instance.PlayAudio("homerun");
            TextPopUps.instance.ShowPopUp("homerun");
            Invoke("HideBallSprite", Random.Range(0.2f, 0.6f));
            Invoke("SetCameraParent", 2f);
        }
    }

    private void HideBallSprite()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void SetCameraParent()
    {
        GameControl.instance.SetCameraToFollowBall(false);
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
