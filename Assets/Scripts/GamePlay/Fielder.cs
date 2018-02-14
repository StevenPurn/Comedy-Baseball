using System.Collections.Generic;
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
    public Transform startPosition, playPosition;
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
        playPosition = Field.playPositions[position].transform;
        team = GameControl.instance.GetTeamInField();
    }

    private void FixedUpdate()
    {
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
    }

    public void ThrowBall(Vector3 target)
    {
        Field.ball.TemporarilyDisableCollision();
        Field.ball.AddForceToBall(target * throwSpeed);
        ballInHands = false;
    }

    void MovePlayer(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
    }
}
