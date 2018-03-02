using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Fielder fielder;
    public float pitchSpeed = 5.0f;
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
        Field.ballHasBeenThrown = false;
        fielder.anim.SetBool("isThrowing", true);
        fielder.ballInHands = false;
        ball.TemporarilyDisableCollision();
        ball.curHeight = 1f;
        ball.curSpeed = pitchSpeed;
        ball.maxHeight = 3.0f;
        ball.startPoint = ball.transform.position;
        ball.endPoint = Field.fielders.Find(x => x.position == Fielder.Position.catcher).glove.position;
    }
}
