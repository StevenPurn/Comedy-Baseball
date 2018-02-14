using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Fielder fielder;
    public float pitchSpeed = 2.0f;
    private Ball ball;
    public enum Pitches { strike, foul, ball, hit};

    private void Start()
    {
        ball = Field.ball.GetComponent<Ball>();
        fielder = GetComponent<Fielder>();
    }

    public void ThrowPitch(Pitches pitchType)
    {
        fielder.anim.SetBool("isThrowing", true);
        Vector2 dir = Field.fielders.Find(x => x.position == Fielder.Position.catcher).transform.position;
        ball.curPitch.type = pitchType;
        ball.AddForceToBall(dir * pitchSpeed);
    }
}
