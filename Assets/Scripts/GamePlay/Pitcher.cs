using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Fielder fielder;
    public float pitchSpeed = 5.0f;
    private Ball ball;
    public enum Pitches { strike, ball, hit, homerun, grandslam };

    private void Start()
    {
        ball = Field.ball.GetComponent<Ball>();
        fielder = GetComponent<Fielder>();
        ball.TemporarilyDisableCollision();
        fielder.ballInHands = true;
        ball.anim.SetBool("Moving", false);
    }

    public void ThrowPitch()
    {
        if(fielder.ballInHands == true)
        {
            GameControl.curInning.pitchesThrownThisInning += 1;
            Field.ballHasBeenThrown = false;
            fielder.anim.SetBool("isThrowing", true);
            fielder.ballInHands = false;
            ball.TemporarilyDisableCollision(0.2f);
            ball.curHeight = 1f;
            ball.curSpeed = pitchSpeed;
            ball.maxHeight = 3.0f;
            ball.startPoint = ball.transform.position;
            ball.shownHomeRunPopup = false;
            fielder.team.pitches += 1;
            Fielder catcher = Field.fielders.Find(x => x.position == Fielder.Position.catcher);
            ball.endPoint = catcher.glove.position;
            ball.targetFielder = catcher;
        }
        else
        {
            Field.fielders[1].ThrowBall(fielder);
        }
    }
}
