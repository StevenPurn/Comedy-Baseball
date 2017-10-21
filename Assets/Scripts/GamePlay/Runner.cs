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
            if (CheckEqual(rb.position, new Vector2(target.transform.position.x, target.transform.position.y),0.1f))
            {
                target = null;
                Field.bases[currentBase].isOccupied = false;
                if(currentBase == 3)
                {
                    GameControl.instance.ChangeTeamScore(1);
                    RemoveRunner();
                }
                else
                { 
                    currentBase += 1;
                    Field.bases[currentBase].isOccupied = true;
                }
            }
        }

        if(currentBase >= 4)
        {
            GameControl.instance.ChangeTeamScore(1);
        }
    }

    bool CheckEqual(Vector2 v1, Vector2 v2, float tolerance)
    {
        if (Mathf.Abs(v1.x - v2.x) < tolerance && Mathf.Abs(v1.y - v2.y) < tolerance)
        {
            return true;
        }
        else return false;
    }

    public void SetBaseAsTarget(int baseToTarget)
    {
        target = Field.bases[baseToTarget].baseObj;
    }

    public void SetOut()
    {
        isOut = true;
        RemoveRunner();
    }

    public void RemoveRunner()
    {
        Field.runners.Remove(this);
        Destroy(gameObject);
    }
}
