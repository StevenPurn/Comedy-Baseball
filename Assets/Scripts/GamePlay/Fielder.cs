﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fielder : MonoBehaviour {

    public enum Position { pitcher, catcher, firstBaseman, secondBaseman, thirdBaseman, shortstop, rightField, centerField, leftField };
    private Rigidbody2D rb;
    private float movementSpeed = 10f;
    private float throwSpeed = 20f;
    public ActiveTeam team;
    public bool ballInHands;
    public Position position;
    public Transform startPosition;
    public bool inningOver = false;
    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().material = team == GameControl.instance.activeTeams[0] ? GameControl.instance.homeTeamMat : GameControl.instance.awayTeamMat;
    }

    public void SetPosition(Position pos)
    {
        position = pos;
        Init();
    }

    private void Init()
    {
        startPosition = Field.fieldPositions[position].transform;
        team = GameControl.instance.GetTeamInField();
    }

    private void FixedUpdate()
    {
        Vector3 movementTarget = new Vector3(0, 0, 0);
        if (!inningOver)
        {
            if (GameControl.ballInPlay)
            {
                //check if ball is in the players region || should he pursue the ball? 
                if (ballInHands)
                {
                    //determine what to do with the ball, throw it at the base a runner is advancing towards
                }
                else
                {
                    movementTarget = Field.ball.transform.position;
                }
            }
            else if (position == Position.catcher && ballInHands)
            {
                Vector2 dir = Field.GetDirectionToThrowBall(transform.position);
                anim.SetBool("isThrowing", true);
                Field.ball.AddForceToBall(dir * throwSpeed);
                Debug.Log("Throwing ball");
                ballInHands = false;
            }
            else
            {
                movementTarget = startPosition.position;
            }
        }
        else
        {
            movementTarget = team.dugout.transform.position;
        }

        if(Utility.CheckEqual(movementTarget, transform.position, 0.1f))
        {
            //reached destination so do a thing based on where you were going
            //pick up ball? throw ball? destroy yourself?
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
    }

    void ThrowBall(Vector3 target)
    {
        
    }

    void MovePlayer(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
    }
}
