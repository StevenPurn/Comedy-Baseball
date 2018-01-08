using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fielder : MonoBehaviour {

    private Rigidbody2D rb;
    private float movementSpeed = 10f;
    public bool ballInHands;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /*public void SetPosition(FieldPositions pos)
    {
        position = pos;
        Init();
    }*/

    private void Init()
    {
        //do setup stuff here
    }

    private void FixedUpdate()
    {
        if (GameControl.ballInPlay)
        {
            //check if ball is in the players region || should he pursue the ball? 
            if (ballInHands)
            {
                //determine what to do with the ball, throw it at the base a runner is advancing towards
            }
        }        
    }

    //Approximation of position equality to prevent weird rubber banding issues
    bool CheckEqual(Vector2 v1, Vector2 v2, float tolerance)
    {
        if (Mathf.Abs(v1.x - v2.x) < tolerance && Mathf.Abs(v1.y - v2.y) < tolerance)
        {
            return true;
        }
        else return false;
    }
}
