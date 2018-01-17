using UnityEngine;

public class Pitcher : MonoBehaviour {

    public Fielder fielder;
    public float pitchSpeed = 20.0f;

    private void Start()
    {
        fielder = GetComponent<Fielder>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ThrowPitch()
    {
        //Play animation
        //Add force to ball
        //Decide if it should be a strike?
        fielder.anim.SetBool("isThrowing", true);
        Vector2 dir = Field.fielders.Find(x => x.position == Fielder.Position.catcher).transform.position;
        Field.ball.GetComponent<Ball>().AddForceToBall(dir * pitchSpeed);
    }
}
