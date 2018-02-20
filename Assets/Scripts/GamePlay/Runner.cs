using System;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {

    public bool atBat = true;
    public int currentBase = 0;
    public bool isOut;
    public bool exitingField = false;
    public bool isAdvancing;
    public bool enteringField = true;
    public List<GameObject> targetBase = new List<GameObject>();
    private Rigidbody2D rb;
    public float movementSpeed = 3.5f;
    private Collider2D col;
    public ActiveTeam team;
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
            if (UnityEngine.Random.Range(0f, 1f) >= 0.5f)
            {
                anim.SetTrigger("isSwingingBat");
            }
            GameControl.instance.HandleStrike(false);
        }
        else if (ball.curPitch.type == Pitcher.Pitches.foul)
        {
            anim.SetTrigger("isSwingingBat");
            Vector2 angle = ball.curPitch.hitAngle;
            Vector2 force = angle * ball.curPitch.hitSpeed;
            ball.TemporarilyDisableCollision(0.3f);
            ball.AddForceToBall(force);
            GameControl.ballInPlay = true;
            GameControl.instance.HandleStrike(true);
        }
        else
        {
            atBat = false;
            anim.SetTrigger("isSwingingBat");
            Vector2 angle = ball.curPitch.hitAngle;
            Vector2 force = angle * ball.curPitch.hitSpeed;
            ball.TemporarilyDisableCollision(0.3f);
            ball.AddForceToBall(force);
            GameControl.ballInPlay = true;
            GameControl.waitingForNextBatter = true;
            isAdvancing = true;
            col.enabled = false;
        }
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
        if (Field.CanRunnerAdvance(this)) { 
            if (Utility.CheckEqual(movementTarget, transform.position, 0.1f))
            {
                SetAnimationValues(Vector3.zero);
                enteringField = false;
                isAdvancing = false;
                if (exitingField)
                {
                    targetBase.Clear();
                    Field.runners.Remove(this);
                    Destroy(transform.parent.gameObject);
                }
                else if (currentBase == 3)
                {
                    GameControl.instance.ChangeTeamScore(1);
                    exitingField = true;
                }
                if(targetBase.Count > 0)
                {
                    if (targetBase[0].name.Contains("Base"))
                    {
                        currentBase += 1;
                    }
                    targetBase.Remove(targetBase[0]);
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

        if(isAdvancing && targetBase[0].name.Contains("Base"))
        {
            Field.CheckIfRunnerOut(this);
        }
    }

    void MovePlayer(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        SetAnimationValues(direction);
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
    }

    private void SetAnimationValues(Vector3 moveDir)
    {
        anim.SetFloat("xMove", moveDir.x);
        anim.SetFloat("yMove", moveDir.y);
        if(moveDir == Vector3.zero)
        {
            anim.SetBool("isIdle", true);
        }
        else
        {
            anim.SetBool("isIdle", false);
        }
        anim.SetBool("isAtBat", atBat);
    }

    public void SetOut()
    {
        isOut = true;
        exitingField = true;
        for (int i = targetBase.Count - 2; i >= 0; i--)
        {
            targetBase.Remove(targetBase[i]);
        }
    }
}