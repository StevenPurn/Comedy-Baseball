using System;
using System.Collections.Generic;
using UnityEngine;

public class HRDBatter : MonoBehaviour
{

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
    public HRDPlayer player;
    public Animator anim;
    private Ball ball;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        ball = Field.ball.GetComponent<Ball>();
        GetComponent<SpriteRenderer>().material = HRDGameControl.instance.batterMat;
        atBat = true;
        // add battersbox as target
        // targetBase.Add();
        targetBase.Add(team.dugout);
        col = GetComponent<Collider2D>();
    }

    public void SwingBat(bool isStrike)
    {
        if (isStrike)
        {
            if (GameControl.strikes != 2)
            {
                string aud = "strike" + UnityEngine.Random.Range(1, 4);
                TextPopUps.instance.ShowPopUp("strike");
                AudioControl.instance.PlayAudio(aud);
            }
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand >= 0.35f)
            {
                anim.SetTrigger("isSwingingBat");
            }
            GameControl.instance.HandleStrike(false);
        }
        else
        {
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
            ball.hasHitGround = false;
            ball.wasHit = true;
            string aud = "hit" + UnityEngine.Random.Range(1, 3);
            if (ball.curPitch.type == Pitcher.Pitches.homerun)
            {
                ball.HandleHomeRun();
            }
            AudioControl.instance.PlayAudio(aud);
            GameControl.instance.SetCameraToFollowBall(true);
            col.enabled = false;
            //HRDGameControl.playHitAudio
            if (true)
            {
                string pitch;
                int num = ball.curPitch.inputNumber;
                if (num != 10)
                {
                    if (num > 1 && num < 4)
                    {
                        pitch = "thatWasBad";
                    }
                    else if (num < 7)
                    {
                        pitch = "meh";
                    }
                    else
                    {
                        pitch = "nice";
                    }
                    TextPopUps.instance.ShowPopUp(pitch);
                    AudioControl.instance.PlayAudio(pitch);
                }
            }
        }

        if (ball.curPitch.type == Pitcher.Pitches.homerun)
        {
            GameControl.isHomeRun = true;
        }
    }

    //Move player towards next base if they have one
    //Use a list of bases to prevent them from moving directly to the final destination
    private void FixedUpdate()
    {
        Vector3 movementTarget = new Vector3(0, 0, 0);
        if (targetBase.Count > 0)
        {
            movementTarget = targetBase[0].transform.position;
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
        if (moveDir == Vector3.zero)
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
        atBat = false;
        isOut = true;
        exitingField = true;
        for (int i = targetBase.Count - 2; i >= 0; i--)
        {
            targetBase.Remove(targetBase[i]);
        }
    }
}