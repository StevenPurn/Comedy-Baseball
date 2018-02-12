using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    public bool isStrike;
    public float curHeight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AddForceToBall(Vector2 force)
    {
        rb.AddForce(force);
    }

    void FixedUpdate ()
    {
        //Can potentially be used for throws if you don't want randomly dropped balls or anything
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.transform.tag;
        if (tag == "Fielder")
        {
            collision.GetComponent<Fielder>().ballInHands = true;
            rb.velocity = Vector2.zero;
        }else if(tag == "Runner")
        {
            collision.GetComponent<Runner>().SwingBat(isStrike);
        }
    }
}
