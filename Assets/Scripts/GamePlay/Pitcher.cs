﻿using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Fielder fielder;
    public float pitchSpeed = 2.0f;
    private Ball ball;
    public enum Pitches { strike, foul, ball, hit, popfly, groundOut, homerun };

    private void Start()
    {
        ball = Field.ball.GetComponent<Ball>();
        fielder = GetComponent<Fielder>();
        fielder.ballInHands = true;
    }

    public void ThrowPitch()
    {
        fielder.anim.SetBool("isThrowing", true);
        fielder.ballInHands = false;
        Vector2 dir = Field.fielders.Find(x => x.position == Fielder.Position.catcher).glove.position - Field.ball.transform.position;
        ball.curHeight = 1f;
        ball.AddForceToBall(dir * pitchSpeed);
    }
}
