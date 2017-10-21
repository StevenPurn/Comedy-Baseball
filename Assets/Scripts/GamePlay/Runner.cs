using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {

    public bool atBat;
    public int currentBase = 0;
    public bool isOut;
    private GameObject target;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Move player towards next base if they have one
    private void FixedUpdate()
    {
        if(target != null)
        {
            rb.MovePosition(target.transform.position);
        }

        if(rb.position == new Vector2(target.transform.position.x, target.transform.position.y))
        {
            target = null;
            currentBase += 1;
        }
    }

    public void SetBaseAsTarget(int baseToTarget)
    {
        target = Field.bases[baseToTarget].baseObj;
    }

    public void SetOut()
    {
        isOut = true;
        Field.runners.Remove(this);
        Destroy(gameObject);
    }
}
