﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fielder : MonoBehaviour {

    public enum Position { pitcher, catcher, firstBaseman, secondBaseman, thirdBaseman, shortstop, rightField, centerField, leftField };
    public float distanceTolerance;
    private Rigidbody2D rb;
    public float movementSpeed = 3.5f;
    private float throwSpeed = 1.8f;
    public Vector3 movementTarget;
    public ActiveTeam team;
    public bool ballInHands;
    public Position position;
    public Transform startPosition, playPosition, glove;
    public bool inningOver = false;
    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().material = team == GameControl.instance.activeTeams[0] ? GameControl.instance.homeTeamMat : GameControl.instance.awayTeamMat;
        //GetComponent<SpriteRenderer>().material.renderQueue = team == GameControl.instance.activeTeams[0] ? 3001 : 3000;
    }

    public void SetPosition(Position pos)
    {
        position = pos;
        Init();
    }

    private void Init()
    {
        startPosition = Field.fieldPositions[position].transform;
        playPosition = Field.playPositions[position].transform;
        team = GameControl.instance.GetTeamInField();
    }

    private void FixedUpdate()
    {
        anim.SetBool("isIdle", true);
        if (!inningOver)
        {
            
        }
        else
        {
            movementTarget = team.dugout.transform.position;
        }

        if(Utility.CheckEqual(movementTarget, transform.position, 0.1f))
        {
            if (inningOver)
            {
                Field.fielders.Remove(this);
                Destroy(gameObject);
            }
        }
        else
        {
            MovePlayer(movementTarget);
        }
        if (ballInHands)
        {
            Field.ball.transform.parent = glove.gameObject.transform;
            Field.ball.transform.localPosition = Vector3.zero;
        }
        else
        {
            Field.ball.transform.parent = Field.fieldParent;
        }
    }

    public void ThrowBall(Vector3 target)
    {
        Field.ball.TemporarilyDisableCollision(0.3f);
        Field.ball.AddForceToBall(target * throwSpeed);
        ballInHands = false;
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
        if (moveDir == Vector3.zero)
        {
            anim.SetBool("isIdle", true);
        }
        else
        {
            anim.SetBool("isIdle", false);
        }
    }
}
