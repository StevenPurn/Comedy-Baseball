using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fielder : MonoBehaviour {

    public enum Position { pitcher, catcher, firstBaseman, secondBaseman, thirdBaseman, shortstop, rightField, centerField, leftField };
    public float distanceTolerance = 0.02f;
    private Rigidbody2D rb;
    public float movementSpeed = 2.5f;
    public float throwSpeed = 4.5f;
    public Vector3 movementTarget;
    public ActiveTeam team;
    public bool ballInHands;
    public Position position;
    public RuntimeAnimatorController catcherController;
    public Transform startPosition, playPosition, glove;
    public bool inningOver = false;
    public Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().material = team == GameControl.instance.activeTeams[0] ? GameControl.instance.homeTeamMat : GameControl.instance.awayTeamMat;
        ballInHands = false;
        if(position == Position.catcher)
        {
            anim.runtimeAnimatorController = catcherController;
        }
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
        if (inningOver)
        {
            movementTarget = team.dugout.transform.position;
        }

        if(Utility.CheckEqual(movementTarget, transform.position, distanceTolerance))
        {
            SetAnimationValues(Vector3.zero);
            if (inningOver)
            {
                Field.fielders.Remove(this);
                ballInHands = false;
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            MovePlayer(movementTarget);
        }
    }

    public void ThrowBall(Fielder target)
    {
        Field.ballHasBeenThrown = true;
        Field.ball.TemporarilyDisableCollision(0.3f);
        Field.ball.curSpeed = throwSpeed;
        Field.ball.maxHeight = 3.0f;
        Field.ball.endPoint = target.glove.transform.position;
        Field.ball.startPoint = Field.ball.transform.position;
        Field.ball.targetFielder = target;
        Field.ball.curPitch.type = Pitcher.Pitches.hit;
        Debug.Log(transform.parent.name + " throwing to " + target.transform.parent.name);
        Debug.Log(target.glove.transform.position);
        ballInHands = false;
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
