using System;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {

    public bool atBat = true;
    public int currentBase = 0;
    public bool isOut;
    public bool exitingField = false;
    public bool isAdvancing;
    private bool addedHit;
    public bool enteringField = true;
    public List<GameObject> targetBase = new List<GameObject>();
    private Rigidbody2D rb;
    public float movementSpeed = 3.0f;
    private Collider2D col;
    public ActiveTeam team;
    public ActivePlayer player;
    public Animator anim;
    private Ball ball;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        ball = Field.ball.GetComponent<Ball>();
        GetComponent<SpriteRenderer>().material = team == GameControl.instance.activeTeams[0] ? GameControl.instance.homeTeamMat : GameControl.instance.awayTeamMat;
        atBat = true;
        foreach (var item in Field.runnerTargets)
        {
            targetBase.Add(item);
        }
        targetBase.Add(team.dugout);
        col = GetComponent<Collider2D>();
    }

    public void SwingBat(bool isStrike)
    {
        //Check if this should be a strike or a hit
        if (isStrike)
        {
            if (GameControl.strikes != 2)
            {
                string aud = "strike" + UnityEngine.Random.Range(1, 4);
                TextPopUps.instance.ShowPopUp("strike");
                AudioControl.instance.PlayAudio(aud);
            } else
            {
                player.ChangeAtBats(1);
            }
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand >= 0.35f)
            {
                anim.SetTrigger("isSwingingBat");
            }
            GameControl.instance.HandleStrike(false);
        }
        //Revisit this if we decide to implement foul balls
        //else if (ball.curPitch.type == Pitcher.Pitches.foul)
        //{
        //    anim.SetTrigger("isSwingingBat");
        //    int pitchIndex = UnityEngine.Random.Range(0, ball.curPitch.hitLoc.Count);
        //    Vector2 hitTarget = ball.curPitch.hitLoc[pitchIndex].center.transform.position;
        //    hitTarget.x += UnityEngine.Random.Range(ball.curPitch.hitLoc[pitchIndex].minOffset.x, ball.curPitch.hitLoc[pitchIndex].maxOffset.x);
        //    hitTarget.y += UnityEngine.Random.Range(ball.curPitch.hitLoc[pitchIndex].minOffset.y, ball.curPitch.hitLoc[pitchIndex].maxOffset.y);
        //    ball.TemporarilyDisableCollision(0.3f);
        //    ball.curSpeed = ball.curPitch.hitSpeed;
        //    ball.maxHeight = ball.curPitch.maxHeight;
        //    ball.endPoint = hitTarget;
        //    ball.startPoint = ball.transform.position;
        //    GameControl.ballInPlay = true;
        //    GameControl.instance.HandleStrike(true);
        //}
        else
        {
            atBat = false;
            anim.SetTrigger("isSwingingBat");
            int pitchIndex = UnityEngine.Random.Range(0, ball.curPitch.hitLoc.Count);
            Vector2 hitTarget = ball.curPitch.hitLoc[pitchIndex].center.transform.position;
            hitTarget.x += UnityEngine.Random.Range(ball.curPitch.hitLoc[pitchIndex].minOffset.x, ball.curPitch.hitLoc[pitchIndex].maxOffset.x);
            hitTarget.y += UnityEngine.Random.Range(ball.curPitch.hitLoc[pitchIndex].minOffset.y, ball.curPitch.hitLoc[pitchIndex].maxOffset.y);
            ball.TemporarilyDisableCollision(0.3f);
            ball.curSpeed = ball.curPitch.hitSpeed;
            ball.maxHeight = ball.curPitch.maxHeight;
            ball.targetFielder = null;
            ball.startPoint = ball.transform.position;
            ball.endPoint = hitTarget;
            ball.hasntHitGround = true;
            string aud = "hit" + UnityEngine.Random.Range(1, 3);
            if(ball.curPitch.type == Pitcher.Pitches.homerun)
            {
                ball.HandleHomeRun();
            }
            Field.mostRecentBatter = this;
            Invoke("FieldersCanReact", 0.2f);
            AudioControl.instance.PlayAudio(aud);
            GameControl.instance.ResetCount();
            GameControl.instance.SetCameraToFollowBall(true);
            GameControl.ballInPlay = true;
            GameControl.waitingForNextBatter = true;
            isAdvancing = true;
            col.enabled = false;
            player.ChangeAtBats(1);
        }

        if(ball.curPitch.type == Pitcher.Pitches.homerun)
        {
            GameControl.isHomeRun = true;
        }
    }

    private void FieldersCanReact()
    {
        Field.fieldersCanReact = true;
    }

    //Move player towards next base if they have one
    //Use a list of bases to prevent them from moving directly to the final destination
    private void FixedUpdate()
    {
        Vector3 movementTarget = new Vector3(0, 0, 0);
        if(targetBase.Count > 0)
        {
            movementTarget = targetBase[0].transform.position;
        }
        //Check if they are exiting the field and then add the dugout as the target
        //Check how far fielders are away from the ball
        if (Field.CanRunnerAdvance(this)) { 
            if (Utility.CheckEqual(movementTarget, transform.position, 0.05f))
            {
                SetAnimationValues(Vector3.zero);
                enteringField = false;
                isAdvancing = false;
                if (exitingField)
                {
                    targetBase.Clear();
                    if (Field.runners.Contains(this))
                    {
                        Field.runners.Remove(this);
                    }
                    Destroy(transform.parent.gameObject);
                }
                else if (currentBase == 3)
                {
                    player.ChangeRuns(1);
                    GameControl.instance.ChangeTeamScore(1);
                    exitingField = true;
                }
                if(targetBase.Count > 0)
                {
                    if (targetBase[0].name.Contains("Base"))
                    {
                        if (GameControl.isHomeRun == false && Vector2.Distance(transform.position, Field.ball.transform.position) < 1f)
                        {
                            AudioControl.instance.PlayAudio("safe");
                        }
                        currentBase += 1;
                    }
                    targetBase.Remove(targetBase[0]);
                }

                if(addedHit == false && atBat == false && exitingField == false)
                {
                    player.ChangeHits(1);
                    addedHit = true;
                }
            }
            else
            {
                if(!(enteringField || exitingField))
                {
                    isAdvancing = true;
                }
                MovePlayer(movementTarget);
            }
        }

        if(isAdvancing && (targetBase[0].name.Contains("Base") || targetBase[0].name.Contains("Plate")))
        {
            Field.CheckIfRunnerOut(this);
        }
    }

    void MovePlayer(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        SetAnimationValues(direction);
        rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
    }

    private void SetAnimationValues(Vector3 moveDir)
    {
        anim.SetFloat("xMove", moveDir.x);
        anim.SetFloat("yMove", moveDir.y);
        if(moveDir == Vector3.zero)
        {
            anim.SetBool("isAtBat", atBat);
            anim.SetBool("isIdle", true);
        }
        else
        {
            anim.SetBool("isAtBat", false);
            anim.SetBool("isIdle", false);
        }
    }

    public void SetOut()
    {
        if (col.enabled)
        {
            col.enabled = false;
        }
        Field.runners.Remove(this);
        atBat = false;
        isOut = true;
        exitingField = true;
        for (int i = targetBase.Count - 2; i >= 0; i--)
        {
            targetBase.Remove(targetBase[i]);
        }
    }
}