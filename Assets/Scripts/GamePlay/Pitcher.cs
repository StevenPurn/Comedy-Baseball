using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Fielder fielder;
    public float pitchSpeed = 20.0f;
    private Ball ball;
    public enum Pitches { strike, foul, ball, hit};

    private void Start()
    {
        ball = Field.ball.GetComponent<Ball>();
        fielder = GetComponent<Fielder>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ThrowPitch(Pitches pitchType)
    {
        //Play animation
        //Add force to ball
        //Decide if it should be a strike?
        fielder.anim.SetBool("isThrowing", true);
        Vector2 dir = Field.fielders.Find(x => x.position == Fielder.Position.catcher).transform.position;
        ball.isStrike = false;
        switch (pitchType)
        {
            case Pitches.ball:
                break;
            case Pitches.strike:
                ball.isStrike = true;
                break;
            case Pitches.foul:
                break;
            case Pitches.hit:
                break;
        }

        ball.AddForceToBall(dir * pitchSpeed);
    }
}
