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
    private float movementSpeed = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Move player towards next base if they have one
    private void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
            if (rb.position == new Vector2(target.transform.position.x, target.transform.position.y))
            {
                target = null;
                currentBase += 1;
            }
        }

        if(currentBase >= 4)
        {
            GameControl.instance.ChangeTeamScore(1);
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
