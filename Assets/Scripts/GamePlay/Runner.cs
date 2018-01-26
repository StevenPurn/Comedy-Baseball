using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {

    public bool atBat = true;
    public int currentBase = 0;
    public bool isOut;
    public bool exitingField = false;
    public bool isAdvancing;
    public List<GameObject> targetBase = new List<GameObject>();
    private Rigidbody2D rb;
    private float movementSpeed = 10f;
    public ActiveTeam team;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().material = team == GameControl.instance.activeTeams[0] ? GameControl.instance.homeTeamMat : GameControl.instance.awayTeamMat;
        atBat = true;
    }

    public void SwingBat()
    {
        bool isStrike = true;
        //Check if this should be a strike or a hit
        if (isStrike)
        {
            if (UnityEngine.Random.Range(0, 1) >= 0.4f)
            {
                anim.SetBool("isSwingingBat", true);
            }
        }
        else
        {
            anim.SetBool("isSwingingBat", true);
            //hit the ball
        }
    }

    //Move player towards next base if they have one
    //Use a list of bases to prevent them from moving directly to the final destination
    private void FixedUpdate()
    {
        //If we still have another base to move towards
        if (targetBase.Count > 0)
        {
            Vector3 movementTarget = new Vector3(0, 0, 0);
            movementTarget = targetBase[0].transform.position;
            if (Utility.CheckEqual(movementTarget, transform.position, 0.1f))
            {
                isAdvancing = false;
                if (exitingField)
                {
                    targetBase.Clear();
                    Destroy(gameObject);
                }
                else if (currentBase == 3)
                {
                    GameControl.instance.ChangeTeamScore(1);
                    RemoveRunner();
                }
                else
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
                isAdvancing = true;
                MovePlayer(movementTarget);
            }
        }
        else if (targetBase.Count == 0)
        {
            //Totally necessary
            targetBase.Clear();
            isAdvancing = false;
            SetAnimationValues(Vector3.zero);
        }
        if (currentBase >= 4)
        {
            Debug.LogWarning("Someone went way too far. Like, good on them. But you should check it");
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

    public void SetBasesAsTargets(List<GameObject> baseToTarget)
    {
        foreach (var targetBase in baseToTarget)
        {
            this.targetBase.Add(targetBase);
        }
    }

    public void SetBaseAsTarget(GameObject baseToTarget)
    {
        targetBase.Add(baseToTarget);
    }

    public void SetOut()
    {
        isOut = true;
        RemoveRunner();
    }

    public void RemoveRunner()
    {
        atBat = false;
        exitingField = true;
        targetBase.Clear();
        SetBaseAsTarget(team.dugout);
        currentBase = 0;
    }
}
