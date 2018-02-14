using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    private float curHeight = 1.5f;
    public bool popFly = false;
    //Chance of error (possibly from previous play)
    private BoxCollider2D col;
    public Pitch curPitch;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void AddRelativeForceToBall(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        if (force.magnitude > 5.0f)
        {
            popFly = true;
        }
        rb.AddRelativeForce(force, ForceMode2D.Impulse);
    }

    public void AddForceToBall(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void HitBallWithFuckingBat(Vector2 force)
    {
        rb.velocity = Vector2.zero;
        Vector2 pos = transform.position;
        rb.AddForce(pos - force, ForceMode2D.Impulse);
    }

    public void TemporarilyDisableCollision()
    {
        col.enabled = false;
        Invoke("EnableCollision", 0.2f);
    }
    
    public void EnableCollision()
    {
        col.enabled = true;
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
            if (curHeight < 4.0f)
            {
                collision.GetComponentInParent<Fielder>().ballInHands = true;
                rb.velocity = Vector2.zero;
            }
        }else if(tag == "Runner")
        {
            collision.GetComponent<Runner>().SwingBat(curPitch.type == Pitcher.Pitches.strike);
        }
    }
}
