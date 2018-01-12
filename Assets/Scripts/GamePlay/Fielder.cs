using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fielder : MonoBehaviour {

    public enum Position { pitcher, catcher, firstBaseman, secondBaseman, thirdBaseman, shortstop, rightField, centerField, leftField };
    private Rigidbody2D rb;
    private float movementSpeed = 10f;
    public ActiveTeam team;
    public bool ballInHands;
    public Position position;
    public Transform startPosition;
    public GameObject ball;
    public bool inningOver = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        ball = GameObject.Find("ball");
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
                    movementTarget = ball.transform.position;
                }
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
                Destroy(gameObject);
            }
        }
        else
        {
            MovePlayer(movementTarget);
        }
    }

    void MovePlayer(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
    }
}
