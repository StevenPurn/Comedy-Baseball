using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Fielder fielder;
    public float pitchSpeed = 2.0f;
    private Ball ball;
    public enum Pitches { strike, foul, ball, hit, popfly };

    private void Start()
    {
        ball = Field.ball.GetComponent<Ball>();
        fielder = GetComponent<Fielder>();
    }

    public void ThrowPitch(Pitches pitchType)
    {
        fielder.anim.SetBool("isThrowing", true);
        fielder.ballInHands = false;
        Vector2 dir = Field.fielders.Find(x => x.position == Fielder.Position.catcher).glove.position - Field.ball.transform.position;
        ball.curPitch.type = pitchType;
        ball.AddForceToBall(dir * pitchSpeed);
    }
}
